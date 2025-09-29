using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace College_Management_System.Models
{
    public class Admin
    {
        public int STUDENTID { get; set; }
        public string Studentname { get; set; }
        public string RegistrationNo { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        public DateTime DOA { get; set; }
        public string Course { get; set; }
        public long DiscountFees { get; set; }
        public string Pincode { get; set; }
        public string rollnumber { get; set; }
        public string Mobile { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Orphan { get; set; }
        public string Gender { get; set; }
        public string Cast { get; set; }
        public string OSC { get; set; }
        public string Semester { get; set; }
        public string Pschool { get; set; }
        public string Religion { get; set; }
        public string BloodGR { get; set; }
        public long PriviousID { get; set; }
        public string Disease { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        //Father information
        public string FatherName { get; set; }
        public string FNID { get; set; }
        public string Education { get; set; }
        public string FMobile { get; set; }
        public string Profession { get; set; }
        public string Income { get; set; }
         //Mother Information -->
        public string MotherName { get; set; }
        public string MNID { get; set; }
        public string MEducation { get; set; }
        public long MMobile { get; set; }
        public long MProfession { get; set; }
        public long MIncome { get; set; }
        public string PicturePath { get; internal set; }
    }
    public class Teacher
    {
        public int id { get; set; }
        public string TeacherName { get; set; }
        public string TeacherId { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        public DateTime DOJ { get; set; }
        public string EmployeeRole { get; set; }
        public int MonthlySalary { get; set; }
        public string TeacherMobile { get; set; }
        public string FatherName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Experience { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string BloodGR { get; set; }
        public string Education { get; set; }
        public string Address { get; set; }
    }
}