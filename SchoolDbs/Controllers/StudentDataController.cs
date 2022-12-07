using MySql.Data.MySqlClient;
using SchoolDb.Models;
using SchoolDbs.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolDbs.Controllers
{
    public class StudentDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();


        /// <summary>
        /// Contacts the databases and returns a list of Students
        /// </summary>
        /// <example>
        /// GET api/StudentData/ListStudent -->
        /// </example>
        /// <returns>
        /// a list of all Students in the databases
        /// </returns>
        [HttpGet]
        [Route("api/StudentData/ListStudent/{SearchKey}")]
        public IEnumerable<Student> listStudent(string SearchKey)
        {
            //Goal: connect to database
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            Debug.WriteLine("The search key is" + SearchKey);

            //run an sql command "select * from students"
            string query = "select * from students where studentfname like @key or studentlname like @key or (concat(studentfname, ' ', studentlname)) like @key";

            Debug.WriteLine(query);

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Student> Students = new List<Student>();

            while (ResultSet.Read())
            {
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudnetLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);

                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudnetLname;
                NewStudent.StudentNumber = StudentNumber;

                Students.Add(NewStudent);
            }

                //go through the result set
                //for each row in the result set, add the students
                Conn.Close();
                return Students;
        }

        /// <summary>
        /// Grabs a particular student from the database given the ID
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns>
        /// the selected student and shows thier info.
        /// </returns>
        [HttpGet]
        [Route("api/studentdata/findstudent/{studentid}")]

        public Student FindStudent(int StudentId)
        {
            //Goal: connect to database
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            //run an sql command "select * from articles"
            string query = "select * from students where studentid =" + StudentId;
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText= query;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            Student SelectedStudent = new Student();
            while (ResultSet.Read())
            {
                SelectedStudent.StudentId = Convert.ToInt32(ResultSet["studentid"]);
                SelectedStudent.StudentFname = ResultSet["studentfname"].ToString();
                SelectedStudent.StudentLname = ResultSet["studentlname"].ToString();
                SelectedStudent.StudentNumber = ResultSet["studentnumber"].ToString();
            }

            Conn.Close();
            return SelectedStudent;
        }
    }
}
