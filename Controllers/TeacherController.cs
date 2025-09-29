using College_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College_Management_System.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        private string conStr = "Data Source=DESKTOP-6HR46AF\\SQLEXPRESS;Initial Catalog=CollegeMS;Integrated Security=True;Encrypt=False";

        public ActionResult Dashboard()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM Teacher WHERE Email=@Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                Session["TeacherName"] = dt.Rows[0]["TeacherName"].ToString();
            }

            return View(dt);
        }
        public ActionResult updateteacher(int? id)
        {
            if (id == null) return RedirectToAction("teacherlist");

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("teacher_update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count == 0)
            {
                return Content("<script>alert('Teacher Not found');location.href='/Teacher/Dashboard'</script>");
            }

            DataRow row = dt.Rows[0];
            Teacher teacher = new Teacher
            {
                id = Convert.ToInt32(row["id"]),
                TeacherName = row["TeacherName"].ToString(),
                Email = row["Email"].ToString(),
                DOJ = Convert.ToDateTime(row["DOJ"]),
                EmployeeRole = row["EmployeeRole"].ToString(),
                MonthlySalary = Convert.ToInt32(row["MonthlySalary"]),
                DOB = Convert.ToDateTime(row["DOB"]),
                TeacherMobile = row["TeacherMobile"].ToString(),
                FatherName = row["FatherName"].ToString(),
                Gender = row["Gender"].ToString(),
                Experience = row["Experience"].ToString(),
                Religion = row["Religion"].ToString(),
                BloodGR = row["BloodGR"].ToString(),
                Education = row["Education"].ToString(),
                Address = row["Address"].ToString()
            };

            return View(teacher); // <- ab Teacher model pass ho raha hai
        }


        [HttpPost]
        public ActionResult updateteacher(Teacher request)
        {
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand("teacher_upda", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.id);
                cmd.Parameters.AddWithValue("@TeacherName", request.TeacherName);
                cmd.Parameters.AddWithValue("@DOJ", request.DOJ.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EmployeeRole", request.EmployeeRole);
                cmd.Parameters.AddWithValue("@MonthlySalary", request.MonthlySalary);
                cmd.Parameters.AddWithValue("@TeacherMobile", request.TeacherMobile);
                cmd.Parameters.AddWithValue("@FatherName", request.FatherName);
                cmd.Parameters.AddWithValue("@DOB", request.DOB.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Email", request.Email);
                cmd.Parameters.AddWithValue("@Experience", request.Experience);
                cmd.Parameters.AddWithValue("@Gender", request.Gender);
                cmd.Parameters.AddWithValue("@BloodGR", request.BloodGR);
                cmd.Parameters.AddWithValue("@Religion", request.Religion);
                cmd.Parameters.AddWithValue("@Education", request.Education);
                cmd.Parameters.AddWithValue("@Address", request.Address);
                con.Open();
                int res = cmd.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    return Content("<script>alert('Data updated ');location.href='/Teacher/Dashboard'</script>");
                }
                else
                {
                    return Content("<script>alert('Data not  updated ');location.href='/Teacher/Dashboard'</script>");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        public ActionResult teacherlist()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM Teacher WHERE Email=@Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                Session["TeacherName"] = dt.Rows[0]["TeacherName"].ToString();
            }

            return View(dt);
        }
    }
}