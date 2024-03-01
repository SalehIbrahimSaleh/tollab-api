using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentDepartment")]
    public class StudentDepartment : DataModel
    {
        

        [Display(Name = "Student")]
        [Required]
        public long? StudentId { get; set; }

        [Display(Name = "Department")]
        [Required]
        public long? DepartmentId { get; set; }
 
        public Student Student { get; set; }
        public Department Department { get; set; }
    }
}
