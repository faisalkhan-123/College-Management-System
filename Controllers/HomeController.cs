using College_Management_System.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using System.Xml.Linq;

namespace College_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private string conStr = "Data Source=DESKTOP-6HR46AF\\SQLEXPRESS;Initial Catalog=CollegeMS;Integrated Security=True;Encrypt=False";
        
        public ActionResult Index()
        {
                return View();
        }
        [HttpPost]
        public ActionResult Index(string firstname,string lastname,string email,string number,string message)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("contactCM", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
                con.Open();    cmd.Parameters.AddWithValue("firstname", firstname);
                cmd.Parameters.AddWithValue("lastname", lastname);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("number", number);
                cmd.Parameters.AddWithValue("message", message);
                int res = cmd.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    ViewBag.contactMessage = "Thanks Contact";
                }
                else
                {
                    return Content("<script>alert('Data Not Submit');location.href='/home/index'</script>");
                }
            }
            return View();
        }

        public ActionResult About()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("List_contact", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);
        }
        public ActionResult update(int? contactId)
        {
            if (contactId == null) return RedirectToAction("about");
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("update_contact", con);
            cmd.Parameters.AddWithValue("contactId", contactId);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            if(dt.Rows.Count==0)
            {
                return Content("<script>alert('Student Not found');location.href='/home/update'</script>");
            }
                return View(dt.Rows[0]);
        }
        [HttpPost]
        public ActionResult update(string firstname, string lastname, string email, string number, string message,int? contactId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("upda_contact", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open(); cmd.Parameters.AddWithValue("firstname", firstname);
                cmd.Parameters.AddWithValue("contactId", contactId);
                cmd.Parameters.AddWithValue("lastname", lastname);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("number", number);
                cmd.Parameters.AddWithValue("message", message);
                int res = cmd.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    return Content("<script>alert('Data  updated ');location.href='/home/update'</script>");
                }
                else
                {
                    return Content("<script>alert('Data not Updated ');location.href='/home/update'</script>");
                }
            }
        }


        public ActionResult delete(int? contactId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("Delete_contact", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("contactId", contactId);
                int res = cmd.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    return RedirectToAction("about");
                }
                else
                {
                    return RedirectToAction("about");
                }
            }
        }
        public ActionResult Sign()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Sign(student request)
        {
            try
            {
                if(ModelState.IsValid)
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
                        if (res > 0)
                        {
                            Session["UserRole"] = "Admin";
                            ViewBag.Success = true;// Form hide karne ke liye flag
                        }
                        else
                        {
                            return Content("<script>alert('Data not Submited Successfully');location.href='/home/Sign'</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SignMessage = ex.Message;
            }
            return View();
        }
        public ActionResult list()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("list_SignUp", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);
        }
        public ActionResult updatesignUp(int? studentId)
        {
            if (studentId == null) return  RedirectToAction("list");
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("update_SignUp",con);
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            if(dt.Rows.Count==0)
            {
                return Content("<script>alert('Student not found');location.href='/home/update'</script>");
            }
            return View(dt.Rows[0]);
        }
        [HttpPost]
        public ActionResult updatesignUp(student request)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("update_Sign", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("studentId", request.studentId);
                cmd.Parameters.AddWithValue("email", request.email);
                cmd.Parameters.AddWithValue("password", request.password);
                cmd.Parameters.AddWithValue("cpassword", request.cpassword);
                cmd.Parameters.AddWithValue("UserRole", request.UserRole);
                con.Open();
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    return Content("<script>alert('Data  updated Successfully');location.href='/home/updatesignUp'</script>");
                }
                else
                {
                    return Content("<script>alert('Data not Submited Successfully');location.href='/home/updatesignUp'</script>");
                }
            }
        }
        public ActionResult deletesignUp(int? studentId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("delete_Sign", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("studentId",studentId);
                con.Open();
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    return Content("<script>alert('Data  deleted Successfully');location.href='/home/list'</script>");
                }
                else
                {
                    return Content("<script>alert('Data not Submited Successfully');location.href='/home/list'</script>");
                }
            }
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string email, string password)
        {
            SqlConnection con = new SqlConnection(conStr);
            string query = "select * from SignUp where email=@email and password=@password";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    FormsAuthentication.SetAuthCookie(email, false);

                    string UserRole = reader["UserRole"].ToString().Trim();
                    Session["UserRole"] = UserRole;

                    // 👇 Yeh line zaroor add karo
                    Session["email"] = reader["email"].ToString().Trim();

                    if (Session["UserRole"].ToString() == "Admin")
                    {
                        Response.Redirect("~/Admin/Dashboard");
                    }
                    else if (Session["UserRole"].ToString() == "Teacher")
                    {
                        Response.Redirect("~/Teacher/Dashboard");
                    }
                    else if (Session["UserRole"].ToString() == "Student")
                    {
                        Response.Redirect("~/Student/Dashboard");
                    }
                }

            }
            else
            {
                return Content("<script>alert('password is increated');location.href='/home/login'</script>");
            }
            return View();
        }
        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("login");
        }

    }
}








//public ActionResult Login(string email, string password)
//{
//    using (SqlConnection con = new SqlConnection(conStr))
//    {
//        string query = "SELECT * FROM SignUp WHERE email=@email AND password=@password";
//        SqlCommand cmd = new SqlCommand(query, con);
//        cmd.Parameters.AddWithValue("@email", email);
//        cmd.Parameters.AddWithValue("@password", password);

//        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//        DataTable dt = new DataTable();
//        adapter.Fill(dt);

//        if (dt.Rows.Count > 0)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                System.Diagnostics.Debug.WriteLine("✅ User is Authenticated: " + User.Identity.Name);
//            }
//            else
//            {
//                System.Diagnostics.Debug.WriteLine("❌ User is NOT Authenticated");
//            }
//            FormsAuthentication.SetAuthCookie(email, false);
//            string userRole = dt.Rows[0]["UserRole"].ToString();
//            Session["UserRole"] = userRole;

//            return RedirectToAction("Dashboard");
//        }
//        else
//        {
//            return Content("<script>alert('Invalid Email or Password');location.href='/home/login'</script>");
//        }
//    }
//}