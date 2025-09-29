using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace College_Management_System.Models
{
    public class student
    {
        public static object StudentName { get; internal set; }
        public int studentId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string cpassword { get; set; }
        public string UserRole { get; set; }
    }
}