using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class WorkoutMethods
    {
        public int CreateWorkout(WorkoutInfo workout, out string error)
        {

            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "INSERT INTO tbl_workout(wo_date,wo_type,wo_user) VALUES(@date,@type,@user)";

            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("date", SqlDbType.Date).Value = workout.Date;
            comm.Parameters.Add("type", SqlDbType.VarChar, 20).Value = workout.TypeOfWorkout;
            comm.Parameters.Add("user", SqlDbType.Int).Value = workout.User.UserId;

            error = "";
            int res = 0;
            try
            {
                conn.Open();
                res = comm.ExecuteNonQuery();
                if (res != 1) error = "Unable to add workout";
                return res;
            }catch(Exception e)
            {
                error = e.Message;
                return res;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateWorkout(WorkoutInfo workout, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Update tbl_workout SET wo_date = @date, wo_type = @type WHERE wo_id = @id";

            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("date", SqlDbType.Date).Value = workout.Date;
            comm.Parameters.Add("type", SqlDbType.VarChar, 20).Value = workout.TypeOfWorkout;
            comm.Parameters.Add("id", SqlDbType.Int).Value = workout.WorkoutId;

            error = "";
            int res = 0;
            try
            {
                conn.Open();
                res = comm.ExecuteNonQuery();
                if (res != 1) error = "Unable to update workout";
                return res;
            }
            catch (Exception e)
            {
                error = e.Message;
                return res;
            }
            finally
            {
                conn.Close();
            }
        }
        public int DeleteWorkout(int id, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "DELETE FROM tbl_workout WHERE wo_id = @id";

            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("id", SqlDbType.Int).Value = id;

            error = "";
            int res = 0;
            try
            {
                conn.Open();
                res = comm.ExecuteNonQuery();
                if (res != 1) error = "Unable to delete workout";
                return res;
            }
            catch (Exception e)
            {
                error = e.Message;
                return res;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<WorkoutInfo> GetWorkouts(int id, User u, out string error)
        {
           
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Select * FROM tbl_workout where wo_user= @id";

            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("id", SqlDbType.Int).Value = id;

            List<WorkoutInfo> workouts = new();
            SqlDataReader read = null;
            error = "";
            try
            {
                conn.Open();
                read = comm.ExecuteReader();

                while (read.Read())
                {
                    WorkoutInfo w = new();
                    w.Date = Convert.ToDateTime(read["wo_date"]);
                    w.TypeOfWorkout = read["wo_type"].ToString();
                    w.User = u;
                    workouts.Add(w);
                }
                read.Close();
                return workouts;

            }catch(Exception e)
            {
                error = e.Message;
                WorkoutInfo err = new();
                err.TypeOfWorkout = "Error";
                workouts.Add(err);
                return workouts;
            }
        }
    }
}
