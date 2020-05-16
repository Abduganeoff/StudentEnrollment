using System;
using System.ComponentModel.DataAnnotations;

namespace Tutorial10.DTOs.Request
{
    public class StudentInsertRequest
    {
        [Required]
        public string IndexNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public int IdEnrollment { get; set; }
    }
}
