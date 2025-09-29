using College_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Management;
namespace College_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private Service studentService = new Service();

        private string conStr = "Data Source=DESKTOP-6HR46AF\\SQLEXPRESS;Initial Catalog=CollegeMS;Integrated Security=True;Encrypt=False";
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult ManageStudents()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManageStudents(student request,Admin obj)
        {
            if (ModelState.IsValid)
            {
                bool success = studentService.Sign(request);

                if (success)
                {
                    Session["UserRole"] = "Student";   // agar admin signup
                                                       //ViewBag.Success = true;        // form hide karne ke liye
                }
                else
                {
                    return Content("<script>alert('Data not Submitted Successfully');" +
                        "location.href='/admin/ManageStudents'</script>");
                }
            }
            string imgPath = "";
            if (obj.Picture != null && obj.Picture.ContentLength > 0)
            {
                string filename = $"{Guid.NewGuid().ToString()}.{obj.Picture.FileName}";
                string folderPath = Server.MapPath("~/update");
                imgPath = Path.Combine(folderPath, filename);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                obj.Picture.SaveAs(imgPath);
                try
                {
                    using(SqlConnection con=new SqlConnection(conStr))
                    {
                        SqlCommand cmd = new SqlCommand("SP_StudentADD", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
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
                        if(res>0)
                        {
                            return Content("<script>alert('data  Successfully');location.href='/Admin/AllStudents'</script>");
                        }
                        else
                        {
                            return Content("<script>alert('Data not Successfully');location.href='/Admin/ManageStudents'<script>");
                        }
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.ADDmessage = ex.Message;
                }
                
            }

            return View();
        }

        public ActionResult AllStudents()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("List_StudentAdd", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);
        }
        public FileResult show(string filename)
        {
            string folderpath = Server.MapPath("~/update");
            string filepath = Path.Combine(folderpath, filename);
            if (!System.IO.File.Exists(filepath))
            {
                return null;
            }
            string contentType = MimeMapping.GetMimeMapping(filepath);
            byte[] fileByte = System.IO.File.ReadAllBytes(filepath);
            return File(fileByte, contentType);
        }
        public ActionResult OnlyListallStudents()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("List_StudentAdd", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);
        }
        public ActionResult Update(int? STUDENTID)
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
                return Content("<script>alert('Student Not Found');location.href='/Admin/OnlyListallStudents'</script>");
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
                DiscountFees=Convert.ToInt64(dt.Rows[0]["DiscountFees"]),
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
                PriviousID =Convert.ToInt64(dt.Rows[0]["PriviousID"]),
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
        public ActionResult Update(Admin obj)
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
                    return Content("<script>alert('Data Updated Successfully');location.href='/Admin/AllStudents'</script>");
                }
                else
                {
                    return Content("<script>alert('Update Failed');location.href='/Admin/Update'</script>");
                }
            }
        }
        public ActionResult Delete(int? STUDENTID)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("Delete_StudentAdd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@STUDENTID",STUDENTID);
                con.Open();
                int res = cmd.ExecuteNonQuery();
                con.Close();

                if (res > 0)
                {
                    return Content("<script>alert('Delete  Successfully');location.href='/Admin/AllStudents'</script>");
                }
                else
                {
                    return Content("<script>alert('Update Failed');location.href='/Admin/OnlyListallStudents'</script>");
                }
            }
            return View();
        }
        public ActionResult PrintBasic()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("List_StudentAdd", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);
        }
        public ActionResult ManageTeachers()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManageTeachers(Teacher request, student reques)
        {
            if (ModelState.IsValid)
            {
                bool success = studentService.Sign(reques);

                if (success)
                {
                    Session["UserRole"] = "Teacher";   // agar admin signup
                                                       //ViewBag.Success = true;        // form hide karne ke liye
                }
                else
                {
                    return Content("<script>alert('Data not Submitted Successfully');" +
                        "location.href='/admin/ManageStudents'</script>");
                }
            }
            string imgPath = "";
            if (request.Picture != null && request.Picture.ContentLength > 0)
            {
                string filename = $"{Guid.NewGuid().ToString()}.{request.Picture.FileName}";
                string folderPath = Server.MapPath("~/updateteacher");
                imgPath = Path.Combine(folderPath, filename);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                request.Picture.SaveAs(imgPath);
                try
                { 
                    SqlConnection con = new SqlConnection(conStr);
                    SqlCommand cmd = new SqlCommand("teachers", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TeacherName", request.TeacherName);
                    cmd.Parameters.AddWithValue("@Picture", filename);
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
                        return Content("<script>alert('Data Submit ');location.href='/admin/Allteacher'</script>");
                    }
                    else
                    {
                        return Content("<script>alert('Data not  Submit ');location.href='/admin/Allteacher'</script>");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View();
        }




        public ActionResult teacherlist()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("teacher_list", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
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
                return Content("<script>alert('Teacher Not found');location.href='/admin/teacherlist'</script>");
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
                        return Content("<script>alert('Data updated ');location.href='/admin/teacherlist'</script>");
                    }
                    else
                    {
                        return Content("<script>alert('Data not  updated ');location.href='/admin/teacherlist'</script>");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            return View();
        }
          
        public FileResult showteacher(string filename)
        {
            string folderpath = Server.MapPath("~/updateteacher");
            string filepath = Path.Combine(folderpath, filename);
            if (!System.IO.File.Exists(filepath))
            {
                return null;
            }
            string contentType = MimeMapping.GetMimeMapping(filepath);
            byte[] fileByte = System.IO.File.ReadAllBytes(filepath);
            return File(fileByte, contentType);
        }
        public ActionResult ManageCourse()
        {
            return View();
        }
        public ActionResult BacisBasic()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("teacher_list", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);
        }
        public ActionResult Allteacher()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("teacher_list", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);
        }

        [HttpPost]
        public JsonResult DeleteTeacher(int id)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Teacher WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                result = cmd.ExecuteNonQuery();
            }
            return Json(result); // return 1 if deleted
        }



        public ActionResult Portal()
        {
            return View();
        }
        public ActionResult ManageSubject()
        {
            return View();
        }
        public ActionResult ManageExams()
        {
            return View();
        }

        public ActionResult ManageResults()
        {
            return View();
        }
        public ActionResult ManageAttendance()
        {
            return View();
        }

        public ActionResult FeesReport()
        {
            return View();
        }
        public ActionResult ManageFees()
        {
            return View();
        }
        public ActionResult TimeTable()
        {
            return View();
        }
    }
}

//public ActionResult ManageTeachers(Teacher request)
//{
//    string imgPath = "";
//    if (request.Picture != null && request.Picture.ContentLength > 0)
//    {
//        string filename = $"{Guid.NewGuid().ToString()}.{request.Picture.FileName}";
//        string folderPath = Server.MapPath("~/updateteacher");
//        imgPath = Path.Combine(folderPath, filename);
//        if (!Directory.Exists(folderPath))
//        {
//            Directory.CreateDirectory(folderPath);
//        }
//        request.Picture.SaveAs(imgPath);
//        try
//        {
//            SqlConnection con = new SqlConnection(conStr);
//            SqlCommand cmd = new SqlCommand("teachers", con);
//            cmd.CommandType = System.Data.CommandType.StoredProcedure;
//            cmd.Parameters.AddWithValue("@TeacherName", request.TeacherName);
//            cmd.Parameters.AddWithValue("@Picture", filename);
//            cmd.Parameters.AddWithValue("@DOJ", request.DOJ);
//            cmd.Parameters.AddWithValue("@EmployeeRole", request.EmployeeRole);
//            cmd.Parameters.AddWithValue("@MonthlySalary", request.MonthlySalary);
//            cmd.Parameters.AddWithValue("@TeacherMobile", request.TeacherMobile);
//            cmd.Parameters.AddWithValue("@FatherName", request.FatherName);
//            cmd.Parameters.AddWithValue("@DOB", request.DOB);
//            cmd.Parameters.AddWithValue("@Email", request.Email);
//            cmd.Parameters.AddWithValue("@Experience", request.Experience);
//            cmd.Parameters.AddWithValue("@Gender", request.Gender);
//            cmd.Parameters.AddWithValue("@BloodGR", request.BloodGR);
//            cmd.Parameters.AddWithValue("@Religion", request.Religion);
//            cmd.Parameters.AddWithValue("@Education", request.Education);
//            cmd.Parameters.AddWithValue("@Address", request.Address);
//            con.Open();
//            int res = cmd.ExecuteNonQuery();
//            con.Close();
//            if (res > 0)
//            {
//                return Content("<script>alert('Data Submit ');location.href='/admin/ManageTeachers'</script>");
//            }
//            else
//            {
//                return Content("<script>alert('Data not  Submit ');location.href='/admin/ManageTeachers'</script>");
//            }
//        }
//        catch (Exception ex)
//        {
//            ViewBag.Error = ex.Message;
//        }
//    }
//    return View();
//}



