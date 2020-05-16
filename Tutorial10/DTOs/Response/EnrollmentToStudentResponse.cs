using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial10.Entities;

namespace Tutorial10.DTOs.Response
{
    public class EnrollmentToStudentResponse
    {

        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public string Study { get; set; }
        public DateTime StartDate { get; set; }
    }
}
