using SchoolDbs.Controllers;
using SchoolDb.Controllers;
using SchoolDb.Models;
using SchoolDbs.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ActionResult List(string SearchKey)
        {
            StudentDataController Controller = new StudentDataController();
            IEnumerable<Student> Students = Controller.listStudent(SearchKey);

            return View(Students);
        }

        // GET: Student/Show/{StudentId}

        public ActionResult Show(int id)
        {
            StudentDataController Controller = new StudentDataController();
            Student SelectedStudent = Controller.FindStudent(id);

            return View(SelectedStudent);
        }

        // GET: Student/New
        public ActionResult New()
        {
            return View();
        }

        //POST: /Student/Delete/{id}
        [HttpPost]

        public ActionResult Delete(int id)
        {
            StudentDataController controller = new StudentDataController();
            controller.DeleteStudent(id);
            return RedirectToAction("List");
        }

        //POST: /Student/Create
        [HttpPost]

        public ActionResult Create(string StudentFname, string StudentLname, string StudentNumber)
        {
            Debug.WriteLine("I have accessed the Create Method");
            Debug.WriteLine(StudentFname);
            Debug.WriteLine(StudentLname);
            Debug.WriteLine(StudentNumber);

            Student NewStudent = new Student();
            NewStudent.StudentFname = StudentFname;
            NewStudent.StudentLname = StudentLname;
            NewStudent.StudentNumber = StudentNumber;

            StudentDataController controller = new StudentDataController();
            controller.AddTeacher(NewStudent);

            return RedirectToAction("List");
        }

        //GET : /Student/DeleteConfirm/{id}

        public ActionResult DeleteConfirm(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudent(id);

            return View(NewStudent);
        }


        //GET : /Student/Update/{id}
        [HttpGet]

        public ActionResult Update(int Id)
        {
            StudentDataController controller = new StudentDataController();
            Student SelectedStudent = controller.FindStudent(Id);

            return View(SelectedStudent);
        }


        //POST: /Student/Update/{id}
        [HttpPost]
        public ActionResult Update(int id, string StudentFname, string StudentLname, string StudentNumber)
        {
            Student StudentInfo = new Student();
            StudentInfo.StudentFname = StudentFname;
            StudentInfo.StudentLname = StudentLname;
            StudentInfo.StudentNumber = StudentNumber;

            StudentDataController controller = new StudentDataController();
            controller.UpdateStudent(id, StudentInfo);

            return RedirectToAction("Show/" + id);


        }
    }  
}       