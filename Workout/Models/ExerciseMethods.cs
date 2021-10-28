using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class ExerciseMethods
    {
        public int AddExercise(Exercise ex, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Insert Into tbl_exersice(ex_name,ex_category,ex_weight,ex_muscle,ex_workout) VALUES(@name,@cat,@weight,@muscle,@workout)";
            SqlCommand comm = new(query, conn);

            comm.Parameters.Add("name", System.Data.SqlDbType.VarChar, 20).Value = ex.Name;
            comm.Parameters.Add("cat", System.Data.SqlDbType.VarChar, 20).Value = ex.Category;
            comm.Parameters.Add("weight", System.Data.SqlDbType.Decimal).Value = ex.Weight;
            comm.Parameters.Add("muscle", System.Data.SqlDbType.VarChar, 20).Value = ex.Muscle;
            comm.Parameters.Add("workout", System.Data.SqlDbType.Int).Value = ex.Workout;

            error = "";
            int res = 0;
            try
            {
                conn.Open();
                
                res = comm.ExecuteNonQuery();
                if (res != 1) error = "Unable to add exercise";
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
        public int RemoveExercise(int id, out string error)
        {

            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "DELETE FROM tbl_exersice WHERE ex_id = @id";
            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id;

            error = "";
            int res = 0;
            try
            {
                conn.Open();
                res = comm.ExecuteNonQuery();
                if (res != 1) error = "Unable to remove exercice";
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
        public int UpdateExercise(Exercise ex, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Update tbl_exersice SET ex_name = @name, ex_category = @cat, ex_weight = @weight, ex_muscle = @muscle, ex_workout = @workout WHERE ex_id = @id";
            SqlCommand comm = new(query, conn);

            comm.Parameters.Add("name", System.Data.SqlDbType.VarChar, 20).Value = ex.Name;
            comm.Parameters.Add("cat", System.Data.SqlDbType.VarChar, 20).Value = ex.Category;
            comm.Parameters.Add("weight", System.Data.SqlDbType.Decimal).Value = ex.Weight;
            comm.Parameters.Add("muscle", System.Data.SqlDbType.VarChar, 20).Value = ex.Muscle;
            comm.Parameters.Add("workout", System.Data.SqlDbType.Int).Value = ex.Workout;
            comm.Parameters.Add("id", System.Data.SqlDbType.Int).Value = ex.ExerciseId;

            error = "";
            int res = 0;
            try
            {
                conn.Open();
                res = comm.ExecuteNonQuery();
                if (res != 1) error = "Unable to update exercice";
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

        public List<Exercise> GetExercises (int id, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Select * FROM tbl_exersice WHERE ex_workout = @id";
            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id;

            error = "";
            List<Exercise> exercises = new();
            SqlDataReader read = null;

            try
            {
                conn.Open();
                read = comm.ExecuteReader();

                while (read.Read())
                {
                    Exercise exercise = new();
                    exercise.ExerciseId = Convert.ToInt32(read["ex_id"]);
                    exercise.Name = read["ex_name"].ToString();
                    exercise.Category = read["ex_category"].ToString();
                    exercise.Weight = Convert.ToDecimal(read["ex_weight"].ToString());
                    exercise.Muscle = read["ex_muscle"].ToString();
                    exercise.Workout = Convert.ToInt32(read["ex_workout"]);
                    exercises.Add(exercise);
                }
                read.Close();
                return exercises;

            }catch(Exception e)
            {
                error = e.Message;
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public Exercise GetExercise(int id, out string error)
        {

            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Select * FROM tbl_exersice WHERE ex_id = @id";
            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id;

            error = "";
            Exercise exercise = new();
            SqlDataReader read = null;

            try
            {
                conn.Open();
                read = comm.ExecuteReader();

                while (read.Read())
                {
                    
                    exercise.ExerciseId = Convert.ToInt32(read["ex_id"]);
                    exercise.Name = read["ex_name"].ToString();
                    exercise.Category = read["ex_category"].ToString();
                    exercise.Weight = Convert.ToDecimal(read["ex_weight"].ToString());
                    exercise.Muscle = read["ex_muscle"].ToString();
                    exercise.Workout = Convert.ToInt32(read["ex_workout"]);
                    
                }
                read.Close();
                return exercise;

            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
