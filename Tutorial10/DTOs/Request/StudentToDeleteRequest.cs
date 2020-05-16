using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial10.DTOs.Request
{
    public class StudentToDeleteRequest
    {
        [Required]
        public string IndexNumber { get; set; }
    }
}
