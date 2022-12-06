using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Http;

namespace SchoolDbs.Models
{
    public class Student : ApiController
    {
        public int StudentID;
        public string StudentFname;
        public string StudentLname;
        public string StudentNumber;
        public DayOfWeek EnrolDate;

        public Student() { }
    }
}
