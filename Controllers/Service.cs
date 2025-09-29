using College_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace College_Management_System.Controllers
{
    public class Service
    {
            private string conStr = "Data Source=DESKTOP-6HR46AF\\SQLEXPRESS;Initial Catalog=CollegeMS;Integrated Security=True;Encrypt=False";

        // SignUp method
        public bool Sign(student request)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        SqlCommand cmd = new SqlCommand("Sp_SignUp", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("email", request.email);
                        cmd.Parameters.AddWithValue("password", request.password);
                        cmd.Parameters.AddWithValue("cpassword", request.cpassword);
                        cmd.Parameters.AddWithValue("UserRole", request.UserRole);

                        con.Open();
                        int res = cmd.ExecuteNonQuery();
                        con.Close();

                        return res > 0; // true agar successfully insert ho gaya
                    }
                }
                catch
                {
                    throw; // Exception controller me handle hoga
                }
            }
        }

    }
