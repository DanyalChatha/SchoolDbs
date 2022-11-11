﻿using SchoolDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolDb.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher/list
        public ActionResult list()
        {
            TeacherDataController MyController = new TeacherDataController();
            IEnumerable<Teacher> Teachers = MyController.listTeacher();

            return View(Teachers);
        }

        //GET: Teacher/show/{teacherid}
        public ActionResult show(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            Teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }
    }
}