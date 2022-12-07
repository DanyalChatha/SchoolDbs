using MySql.Data.MySqlClient;
using SchoolDbs.Controllers;
using SchoolDb.Controllers;
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
            cmd.CommandText = query;

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

        /// <summary>
        /// Adds an Student to the MySQL Database. 
        /// </summary>
        /// <param name="NewStudent">An object with fields that map to the columns of the Student's table. </param>
        /// <example>
        /// POST api/StudentData/AddStudent 
      	/// <example>
        /// "StudentFname": "Danyal"
        ///	"StudentLname":"Chatha",
        ///	"StudentNumber":"T123",
        ///	</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Student NewStudent)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(NewStudent.StudentFname);

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "insert into Students (studentfname, studentlname, studentnumber) values (@StudentFname,@StudentLname,@StudentNumber)";
            cmd.Parameters.AddWithValue("@StudentFname", NewStudent.StudentFname);
            cmd.Parameters.AddWithValue("@StudentLname", NewStudent.StudentLname);
            cmd.Parameters.AddWithValue("@StudentNumber", NewStudent.StudentNumber);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Deletes an Student from the connected MySQL Database if the ID of that Student exists.
        /// </summary>
        /// <param name="Id">The ID of the Student</param>
        /// <example>POST /api/StudentData/DeleteStudent/2</example> 
        [HttpPost]
        [Route("api/StudentData/DeleteStudent/{Id}")]
        public void DeleteStudent(int Id)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Delete from student where studentid=@id";
            cmd.Parameters.AddWithValue("@id", Id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// Updates an student on the MySQL Database. .
        /// </summary>
        /// <param name="StudentInfo">An object with fields that map to the columns of the student's table.</param>
        /// <example>
        /// POST api/StudentData/UpdateStudent/12 
        /// <example>
        /// "StudentFname": "Danyal"
        ///	"StudentLname":"Chatha",
        ///	"StudentNumber":"T123",
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateStudent(int Id, [FromBody] Student StudentInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Debug.WriteLine(StudentInfo.StudentFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update students set studentfname=@StudentFname, studentlname=@StudentLname, studentnumber=@StudentNumber where studentid=@StudentId";
            cmd.Parameters.AddWithValue("@StudentFname", StudentInfo.StudentFname);
            cmd.Parameters.AddWithValue("@StudentLname", StudentInfo.StudentLname);
            cmd.Parameters.AddWithValue("@studentnumber", StudentInfo.StudentNumber);
            cmd.Parameters.AddWithValue("@StudentId", Id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }
    }
}
