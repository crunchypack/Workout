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
                Users = u,
                Workouts = wm.GetWorkouts(user, logged, out string wError)
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
                if (error.Contains(u.UserName)) error = "Username already exists";
                if (error.Contains(u.Email)) error = "Email already in use";
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
        [HttpGet]
        public IActionResult Workout(int id)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            
            WorkoutMethods wm = new();
            ExerciseMethods em = new();
            
            WorkoutInfo w = wm.GetWorkout(id,user, out string error);
            List<WorkoutInfo> wos = new();
            if(w.WorkoutId == 0) // Check if workout exists
            {
                return RedirectToAction(nameof(Index));
            }
            wos.Add(w);
            ViewModels vm = new()
            {
                Workouts = wos,
                Exercises = em.GetExercises(id, out string Eerror)
            };
            ViewBag.error = error;

            return View(vm);
        }
        [HttpGet]
        public IActionResult EditWorkout (int id)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            WorkoutMethods wm = new();
            WorkoutInfo w = wm.GetWorkout(id, user, out string error);
            if (w.WorkoutId == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.error = error;
            ViewBag.id = id;
            return View(w);

        }
        [HttpPost]
        public IActionResult EditWorkout(IFormCollection fc)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));

            WorkoutMethods wm = new();
            WorkoutInfo w = new();

            w.Date = Convert.ToDateTime(fc["Date"]);
            w.TypeOfWorkout = fc["TypeOfWorkout"];
            w.WorkoutId = Convert.ToInt32(fc["id"]);

            int res = wm.UpdateWorkout(w, out string error);
            ViewBag.error = w.Date;
            ViewBag.id = w.WorkoutId;

            if (res != 0)
            {
                return RedirectToAction("Workout", new { id = w.WorkoutId});
            }
            return View(w);

        }
        [HttpGet]
        public IActionResult DeleteWorkout(int id)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            WorkoutMethods wm = new();
            WorkoutInfo w = wm.GetWorkout(id, user, out string error);
            if (w.WorkoutId == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            int res = wm.DeleteWorkout(id, out string dError);
            if (res == 1)
            {
               return RedirectToAction(nameof(Index));
            }
            ViewBag.error = error + dError;
            
            return View(w);
        }
        [HttpGet]
        public IActionResult AddExercise (int id)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            WorkoutMethods wm = new();
            // Workout
            WorkoutInfo w = wm.GetWorkout(id, user, out string error);
            if (w.WorkoutId == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["Workout"] = w;
            ViewData["Exercise"] = id;
            return View();
        }
        [HttpPost]
        public IActionResult AddExercise(IFormCollection fc)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));

            ExerciseMethods em = new();
            Exercise ex = new();
            string weight = fc["Weight"].ToString().Replace(".", ",");
            ex.Name = fc["Name"];
            ex.Category = fc["Category"];
            ex.Weight = Convert.ToDecimal(weight);
            ex.Muscle = fc["Muscle"];
            ex.Workout = Convert.ToInt32(fc["Workout"]);
            ViewData["Exercise"] = ex.Workout;

            int res = em.AddExercise(ex, out string error);
            
            ViewBag.error = error;

            if(res == 1) return RedirectToAction(nameof(Index));
            return View();
        }
        [HttpGet]
        public IActionResult Exercise(int id)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            WorkoutMethods wm = new();
            List<WorkoutInfo> workouts = wm.GetWorkouts(user, new Models.User(), out string wError);
            ExerciseMethods em = new();
            Exercise ex = em.GetExercise(id, out string error);
            if(!workouts.Any(wo => wo.WorkoutId == ex.Workout))
            {
                return RedirectToAction(nameof(Index));
            }
            WorkoutInfo work = workouts.First(w => w.WorkoutId == ex.Workout);
            ViewBag.error = error + wError;
            ViewData["Workout"] = work;

            return View(ex);
        }
        [HttpGet]
        public IActionResult EditExercise(int id)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            WorkoutMethods wm = new();
            List<WorkoutInfo> workouts = wm.GetWorkouts(user, new Models.User(), out string wError);
            ExerciseMethods em = new();
            Exercise ex = em.GetExercise(id, out string error);
            if (!workouts.Any(wo => wo.WorkoutId == ex.Workout))
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.error = error + wError;
            return View(ex);
        }
        [HttpPost]
        public IActionResult EditExercise(IFormCollection fc)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));

            ExerciseMethods em = new();
            Exercise ex = new();
            string weight = fc["Weight"].ToString().Replace(".", ",");
            ex.ExerciseId = Convert.ToInt32(fc["ExerciseId"]);
            ex.Name = fc["Name"];
            ex.Category = fc["Category"];
            ex.Weight = Convert.ToDecimal(weight);
            ex.Muscle = fc["Muscle"];
            ex.Workout = Convert.ToInt32(fc["Workout"]);
           

            int res = em.UpdateExercise(ex, out string error);

            ViewBag.error = error;

            if (res == 1) return RedirectToAction(nameof(Index));
            return View(ex);
        }
        [HttpGet]
        public IActionResult DeleteExercise(int id)
        {
            int user = (int)HttpContext.Session.GetInt32("user").GetValueOrDefault();
            if (user == 0) return RedirectToAction(nameof(LogInUser));
            WorkoutMethods wm = new();
            List<WorkoutInfo> workouts = wm.GetWorkouts(user, new Models.User(), out string wError);
            ExerciseMethods em = new();
            Exercise ex = em.GetExercise(id, out string error);
            if (!workouts.Any(wo => wo.WorkoutId == ex.Workout))
            {
                return RedirectToAction(nameof(Index));
            }
            int res = em.RemoveExercise(id, out string eError);
            if(res == 1)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.error = error + wError + eError;
            return View(ex);
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
