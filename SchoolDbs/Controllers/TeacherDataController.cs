using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolDb.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;


namespace SchoolDb.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();


        //This Controller Will access the teacher table of our blog database.
        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <returns>
        /// A list of Teacher Objects with fields mapped to the database column values (first name, last name, employee).
        /// </returns>
        /// <example>GET api/TeacherData/ListTeachers -> {Teacher Object, Teacher Object, teacherr Object...}</example>
        [HttpGet]
        [Route("api/TeacherData/ListTeacher/{SearchKey}")]
        public IEnumerable<Teacher> ListTeacher(string SearchKey)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            Debug.WriteLine("The search key is" + SearchKey);


            string query = "select * from teachers where teacherfname like @key or teacherlname like @key or (concat(teacherfname,' ', teacherlname)) like @key";

            Debug.WriteLine(query);

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;


            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();


            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
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

                //Add the Teacher Name to the List
                Teachers.Add(NewTeacher);
            }

            Conn.Close();

            return Teachers;

        }
        /// <summary>
        /// Finds an author from the MySQL Database through an id. 
        /// </summary>
        /// <param name="Id">The Teacher ID</param>
        /// <returns>Teacher object containing information about the author with a matching ID. Empty Author Object if the ID does not match any authors in the system.</returns>
        /// <example>api/TeacherData/FindTeacher/2-> {Teacher Object}</example>
        /// <example>api/TeacherData/FindTeacher/7 -> {Teacher Object}</example>
        [HttpGet]
        [Route("api/teacherdata/findteacher/{Id}")]

        public Teacher FindTeacher(int Id)
        {
            //Goal: connect to database
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            //run an sql command "select * from teachers"
            string query = "select * from teachers where teacherid = @id";
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", Id);
            cmd.Prepare();

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


        /// <summary>
        /// Adds an Teacher to the MySQL Database. 
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the Teacher's table. </param>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
      	/// <example>
        /// "TeacherFname": "Danyal"
        ///	"TeacherLname":"Chatha",
        ///	"EmployeeNumber":"T123",
        ///	"Salary":"800.00"
        ///	</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(NewTeacher.TeacherFname);

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, salary, hiredate) values (@TeacherFname,@TeacherLname,@EmployeeNumber, @Salary, CURRENT_DATE())";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();




        }

        /// <summary>
        /// Updates an Teacher on the MySQL Database. .
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that map to the columns of the author's table.</param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher/12 
        /// <example>
        /// "TeacherFname": "Danyal"
        ///	"TeacherLname":"Chatha",
        ///	"EmployeeNumber":"T123",
        ///	"Salary":"800.00"
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateTeacher(int Id, [FromBody] Teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Debug.WriteLine(TeacherInfo.TeacherFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update teachers set teacherfname=@teacherFname, teacherlname=@teacherLname, employeenumber=@EmployeeNumber, salary=@Salary where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@TeacherId", Id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }


        /// <summary>
        /// Deletes an Author from the connected MySQL Database if the ID of that Teacher exists.
        /// </summary>
        /// <param name="Id">The ID of the teacher</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/3</example> 
        [HttpPost]
        [Route("api/TeacherData/DeleteTeacher/{Id}")]
        public void DeleteTeacher(int Id)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", Id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }
    }

}
