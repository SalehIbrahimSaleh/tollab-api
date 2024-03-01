using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("CourseDepartment")]
    public class CourseDepartment : DataModel
    {

        [Required]
        [Display(Name = "Course")]
        public long? CourseId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public long? DepartmentId { get; set; }

        public Course Course { get; set; }

        public Department Department { get; set; }
    }
}
