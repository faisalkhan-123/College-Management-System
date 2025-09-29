using College_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private string conStr = "Data Source=DESKTOP-6HR46AF\\SQLEXPRESS;Initial Catalog=CollegeMS;Integrated Security=True;Encrypt=False";
        // GET: Student
        public ActionResult Dashboard()
        {

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM StudentADD WHERE Email=@Email"; 
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString()); 

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                Session["StudentName"] = dt.Rows[0]["StudentName"].ToString();
            }

            return View(dt); 
        }
        public ActionResult Updates(int? STUDENTID)
        {
            if (STUDENTID == null)
                return RedirectToAction("OnlyListallStudents");

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM StudentADD WHERE STUDENTID=@STUDENTID", con);
                cmd.Parameters.AddWithValue("@STUDENTID", STUDENTID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count == 0)
            {
                return Content("<script>alert('Student Not Found');location.href='/Student/Dashboard'</script>");
            }

            // row ko model me map karo (Admin object)
            Admin obj = new Admin
            {
                STUDENTID = Convert.ToInt32(dt.Rows[0]["STUDENTID"]),
                Studentname = dt.Rows[0]["Studentname"].ToString(),
                Mobile = dt.Rows[0]["Mobile"].ToString(),
                Email = dt.Rows[0]["Email"].ToString(),
                DOA = Convert.ToDateTime(dt.Rows[0]["DOA"]),
                Course = dt.Rows[0]["Course"].ToString(),
                DiscountFees = Convert.ToInt64(dt.Rows[0]["DiscountFees"]),
                Pincode = dt.Rows[0]["Pincode"].ToString(),
                DOB = Convert.ToDateTime(dt.Rows[0]["DOB"]),
                rollnumber = dt.Rows[0]["rollnumber"].ToString(),
                Orphan = dt.Rows[0]["Orphan"].ToString(),
                Gender = dt.Rows[0]["Gender"].ToString(),
                Cast = dt.Rows[0]["Cast"].ToString(),
                OSC = dt.Rows[0]["OSC"].ToString(),
                Semester = dt.Rows[0]["Semester"].ToString(),
                Pschool = dt.Rows[0]["Pschool"].ToString(),
                Religion = dt.Rows[0]["Religion"].ToString(),
                BloodGR = dt.Rows[0]["BloodGR"].ToString(),
                PriviousID = Convert.ToInt64(dt.Rows[0]["PriviousID"]),
                Disease = dt.Rows[0]["Disease"].ToString(),
                Status = dt.Rows[0]["Status"].ToString(),
                Address = dt.Rows[0]["Address"].ToString(),
                FatherName = dt.Rows[0]["FatherName"].ToString(),
                FNID = dt.Rows[0]["FNID"].ToString(),
                Education = dt.Rows[0]["Education"].ToString(),
                FMobile = dt.Rows[0]["FMobile"].ToString(),
                Profession = dt.Rows[0]["Profession"].ToString(),
                Income = dt.Rows[0]["Income"].ToString(),
                MotherName = dt.Rows[0]["MotherName"].ToString(),
                MNID = dt.Rows[0]["MNID"].ToString(),
                MEducation = dt.Rows[0]["MEducation"].ToString(),
                MMobile = Convert.ToInt64(dt.Rows[0]["MMobile"]),
                MProfession = Convert.ToInt64(dt.Rows[0]["MProfession"]),
                MIncome = Convert.ToInt64(dt.Rows[0]["MIncome"]),
                // baki fields bhi map kar do
            };

            return View(obj);  // View strongly-typed Admin model lega
        }

        // POST: Update Student
        [HttpPost]
        public ActionResult Updates(Admin obj)
        {
            string filename = obj.PicturePath;

            if (obj.Picture != null && obj.Picture.ContentLength > 0)
            {
                filename = $"{Guid.NewGuid()}{Path.GetExtension(obj.Picture.FileName)}";
                string folderpath = Server.MapPath("~/update");
                if (!Directory.Exists(folderpath))
                    Directory.CreateDirectory(folderpath);

                obj.Picture.SaveAs(Path.Combine(folderpath, filename));
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("Upda_StudentAdd", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@STUDENTID", obj.STUDENTID);
                cmd.Parameters.AddWithValue("@Studentname", obj.Studentname);
                cmd.Parameters.AddWithValue("@Picture", (object)filename ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DOA", obj.DOA);
                cmd.Parameters.AddWithValue("@Course", obj.Course);
                cmd.Parameters.AddWithValue("@DiscountFees", (object)obj.DiscountFees ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Pincode", (object)obj.Pincode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Mobile", obj.Mobile);
                cmd.Parameters.AddWithValue("@rollnumber", obj.rollnumber);
                cmd.Parameters.AddWithValue("@DOB", obj.DOB);
                cmd.Parameters.AddWithValue("@Email", obj.Email);
                cmd.Parameters.AddWithValue("@Orphan", (object)obj.Orphan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                cmd.Parameters.AddWithValue("@Cast", (object)obj.Cast ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OSC", (object)obj.OSC ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Semester", (object)obj.Semester ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Pschool", (object)obj.Pschool ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Religion", (object)obj.Religion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BloodGR", (object)obj.BloodGR ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PriviousID", (object)obj.PriviousID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Disease", (object)obj.Disease ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", obj.Status);
                cmd.Parameters.AddWithValue("@Address", (object)obj.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FatherName", obj.FatherName);
                cmd.Parameters.AddWithValue("@FNID", (object)obj.FNID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Education", (object)obj.Education ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FMobile", (object)obj.FMobile ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Profession", (object)obj.Profession ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Income", (object)obj.Income ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MotherName", obj.MotherName);
                cmd.Parameters.AddWithValue("@MNID", (object)obj.MNID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MEducation", (object)obj.MEducation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MMobile", (object)obj.MMobile ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MProfession", (object)obj.MProfession ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MIncome", (object)obj.MIncome ?? DBNull.Value);

                con.Open();
                int res = cmd.ExecuteNonQuery();
                con.Close();
                if (res > 0)
                {
                    return Content("<script>alert('Data Updated Successfully');location.href='/Student/Dashboard'</script>");
                }
                else
                {
                    return Content("<script>alert('Update Failed');location.href='/Student/Dashboard'</script>");
                }
            }
        }
        public ActionResult Print()
        {

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM StudentADD WHERE Email=@Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                Session["StudentName"] = dt.Rows[0]["StudentName"].ToString();
            }

            return View(dt);
        }

        public ActionResult ChangePassword(int? studentId)
        {
            if (studentId == null) return RedirectToAction("Dashboard");
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("update_SignUp", con);
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return Content("<script>alert('Student not found');location.href='/Student/ChangePassword'</script>");
            }
            return View(dt.Rows[0]);
        }
        [HttpPost]
        public ActionResult ChangePassword(student request)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("update_Sign", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("studentId", request.studentId);
                cmd.Parameters.AddWithValue("password", request.password);
                cmd.Parameters.AddWithValue("cpassword", request.cpassword);
                con.Open();
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    return Content("<script>alert('Data  updated Successfully');location.href='/Student/ChangePassword'</script>");
                }
                else
                {
                    return Content("<script>alert('Data not Submited Successfully');location.href='/Student/ChangePassword'</script>");
                }
            }
        }
    }
}