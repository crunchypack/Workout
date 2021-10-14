using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Workout.Models;

namespace Workout.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            UserMethods um = new();
            WorkoutMethods wm = new();
            User logged = um.GetUser(user, out string error);
            List<User> u = new();
            u.Add(logged);
            ViewModels vm = new()
            {
                users = u,
                workouts = wm.GetWorkouts(user, logged, out string wError)
            };
            ViewBag.error = error + wError;
            return View(vm);
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user != 0) return RedirectToAction(nameof(Index));
            return View();
        }
        [HttpPost]
        public IActionResult CreateUser(IFormCollection fc)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user != 0) return RedirectToAction(nameof(Index));
            UserMethods um = new();
            User u = new();
            u.Email = fc["Email"];
            u.UserName = fc["UserName"];
            u.Password = fc["Password"];

            int add = um.InsertUser(u, out string error);
            if(add != 0)
            {
                HttpContext.Session.SetInt32("user", add);        
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.error = error;
                return View();
            }
        }
        [HttpPost]
        public IActionResult DeleteUser(IFormCollection fc)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            UserMethods um = new();
            int res = um.DeleteUser(user, fc["password"], out string error);
            ViewBag.error = error;
            if(res == 1)
            {
                HttpContext.Session.Remove("user");
                return RedirectToAction(nameof(LogInUser));
            }
            return View();

        }
        [HttpGet]
        public IActionResult LogInUser()
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user != 0) return RedirectToAction(nameof(Index));
            return View();
        }
        [HttpPost]
        public IActionResult LogInUser(IFormCollection fc)
        {
            UserMethods um = new();
            User log = um.Authenticate(fc["UserName"], fc["Password"], out string error);
            ViewBag.error = error;
            if (log != null)
            {
                HttpContext.Session.SetInt32("user", log.UserId);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction(nameof(LogInUser));
        }

        [HttpGet]
        public IActionResult CreateWorkout()
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            return View();
        }
        [HttpPost]
        public IActionResult CreateWorkout(IFormCollection fc)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            UserMethods um = new();
            User curr = um.GetUser(user, out string uError);
            WorkoutMethods wm = new();
            WorkoutInfo workout = new();
            workout.Date = Convert.ToDateTime(fc["Date"]);
            workout.TypeOfWorkout = fc["TypeOfWorkout"];
            workout.User = curr;

            int res = wm.CreateWorkout(workout, out string error);
            ViewBag.error = uError + error;
            if (res == 1) return RedirectToAction(nameof(Index));
            else return View();

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
