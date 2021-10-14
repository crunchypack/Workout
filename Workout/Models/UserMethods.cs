using System;
using BC = BCrypt.Net.BCrypt;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class UserMethods
    {
        public UserMethods() { }
        public User GetUser(int id, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "SELECT * FROM tbl_user WHERE u_id = @id";
            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("id", SqlDbType.Int).Value = id;

            SqlDataReader read = null;

            User user = new();

            error = "";
            try
            {
                conn.Open();
                read = comm.ExecuteReader();
                while (read.Read())
                {
                    user.UserId = Convert.ToInt32(read["u_id"]);
                    user.UserName = read["u_username"].ToString();
                    user.Password = read["u_password"].ToString();
                    user.Email = read["u_email"].ToString();
                }
                read.Close();
                return user;
            }catch(Exception e)
            {
                error = e.Message;
                User err = new();
                err.UserName = "Error";
                return err;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertUser(User user, out string error)
        {
            user.Password = BC.HashPassword(user.Password);

            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "INSERT INTO tbl_user(u_username, u_password,u_email) VALUES(@username,@password,@email)";
            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("username", SqlDbType.VarChar,50).Value = user.UserName;
            comm.Parameters.Add("password", SqlDbType.VarChar,100).Value = user.Password;
            comm.Parameters.Add("email", SqlDbType.VarChar,50).Value = user.Email;

            error = "";
            int res = 0;
            try
            {
                conn.Open();
                res = comm.ExecuteNonQuery();
                if (res == 1) error = "";
                else
                {
                    error = "Unable to create user";
                    return res;
                }
                String q = "SELECT * FROM tbl_user WHERE u_username = @create";
                comm = new(q, conn);
                comm.Parameters.Add("create", SqlDbType.VarChar, 50).Value = user.UserName;
                SqlDataReader read = null;

                User u = new();

                error = "";
                
                
                read = comm.ExecuteReader();
                while (read.Read())
                {
                    u.UserId = Convert.ToInt32(read["u_id"]);
                }
                read.Close();
                return u.UserId;
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

        public User Authenticate(string username, string pass, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Select * FROM tbl_user WHERE u_username = @username";
            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("username", SqlDbType.VarChar, 50).Value = username;


            SqlDataReader read = null;

            User user = new();

            error = "";
            try
            {
                conn.Open();
                read = comm.ExecuteReader();
                while (read.Read())
                {
                    user.UserId = Convert.ToInt32(read["u_id"]);
                    user.UserName = read["u_username"].ToString();
                    user.Password = read["u_password"].ToString();
                    user.Email = read["u_email"].ToString();
                }
                if (user.UserName !=  null && BC.Verify(pass, user.Password)) return user;
                else
                {
                    error = "Wrong username or password";
                    return null;
                }
                
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

        public int DeleteUser(int id, string password, out string error)
        {
            SqlConnection conn = new()
            {
                ConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=Workout;Integrated Security=True"
            };
            String query = "Select * FROM tbl_user WHERE u_id = @id";
            SqlCommand comm = new(query, conn);
            comm.Parameters.Add("id", SqlDbType.Int).Value = id;


            SqlDataReader read = null;

            User user = new();

            error = "";
            try
            {
                conn.Open();
                read = comm.ExecuteReader();
                while (read.Read())
                {
                    user.UserId = Convert.ToInt32(read["u_id"]);
                    user.UserName = read["u_username"].ToString();
                    user.Password = read["u_password"].ToString();
                    user.Email = read["u_email"].ToString();
                }
                read.Close();
                if (BC.Verify(password, user.Password))
                {
                    String del = "DELETE FROM tbl_user WHERE u_id = @deleteId";
                    comm = new(del, conn);
                    comm.Parameters.Add("deleteId", SqlDbType.Int).Value = id;
                    int res = 0;
                    res = comm.ExecuteNonQuery();
                    if (res == 1) error = "";
                    else error = "User could not be deleted";
                    return res;
                }
                else
                {
                    error = "Wrong password";
                    return 0;
                }

            }
            catch (Exception e)
            {
                error = e.Message;
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
