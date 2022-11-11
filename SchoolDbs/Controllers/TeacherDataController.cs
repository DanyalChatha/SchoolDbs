using MySql.Data.MySqlClient;
using SchoolDb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolDb.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();


        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeacher</example>
        /// <returns>
        /// A list of Teacher (first names and last names)
        /// </returns>
        [HttpGet]
        public IEnumerable<Teacher> listTeacher()
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();


            string query = "select * from teachers";
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;


            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.Salary = Salary;
                NewTeacher.HireDate = HireDate;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.EmployeeNumber = EmployeeNumber;

                Teachers.Add(NewTeacher);
            }

            Conn.Close();

            return Teachers;

        }

        /// <summary>
        /// Grabs a particular Teacher from the database given the ID
        /// </summary>
        /// <param name="TeacherId"></param>
        /// <returns>
        /// A teachers that from the id the user input
        /// </returns>
        [HttpGet]
        [Route("api/teacherdata/findteacher/{teacherid}")]

        public Teacher FindTeacher(int TeacherId)
        {
            //Goal: connect to database
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            //run an sql command "select * from teachers"
            string query = "select * from teachers where teacherid =" + TeacherId;
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            Teacher SelectedTeacher = new Teacher();
            while (ResultSet.Read())
            {
                SelectedTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.Salary = Convert.ToDecimal(ResultSet["salary"]);
                SelectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.TeacherLname = ResultSet["teacherlname"].ToString();
                SelectedTeacher.TeacherFname = ResultSet["teacherfname"].ToString();

            }
            Conn.Close();

            return SelectedTeacher;

        }

    }
}
