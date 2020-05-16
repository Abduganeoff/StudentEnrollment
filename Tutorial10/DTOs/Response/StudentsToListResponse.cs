using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial10.Entities;

namespace Tutorial10.DTOs.Response
{
    public class StudentsToListResponse
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age{ get; set; }

        public virtual EnrollmentToStudentResponse IdEnrollmentNavigation { get; set; }
    }
}
