using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class ExerciseMethods
    {
        public int AddExercise(Exercise ex,int id, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Insert Into tbl_exercise(ex_name,ex_category,ex_weight,ex_muscle,ex_workout) VALUES(@name,@cat,@weight,@muscle,@workout)";
            SqlCommand comm = new(query, conn);

            comm.Parameters.Add("name", System.Data.SqlDbType.VarChar, 20).Value = ex.Name;
            comm.Parameters.Add("cat", System.Data.SqlDbType.VarChar, 20).Value = ex.Category;
            comm.Parameters.Add("weight", System.Data.SqlDbType.Decimal).Value = ex.Weight;
            comm.Parameters.Add("muscle", System.Data.SqlDbType.VarChar, 20).Value = ex.Muscle;
            comm.Parameters.Add("workout", System.Data.SqlDbType.Int).Value = ex.Workout.WorkoutId;

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
    }
}
