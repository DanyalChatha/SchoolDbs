using SchoolDbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolDbs.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/List
        public ActionResult List()
        {
            StudentDataController Controller = new StudentDataController();
            IEnumerable<Student> Students = Controller.listStudent();

            return View(Students);
        }

        // GET: Student/Show/{StudentId}

        public ActionResult Show(int id)
        {
            StudentDataController Controller = new StudentDataController();
            Student SelectedStudent = Controller.FindStudent(id);

            return View(SelectedStudent);
        }
    }  
}