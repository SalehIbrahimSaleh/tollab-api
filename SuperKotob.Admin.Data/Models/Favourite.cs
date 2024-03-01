using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Favourite")]
    public class Favourite : DataModel
    {

        [Required]
        [Display(Name = "Student")]
        public long? StudentId { get; set; }

        [Required]
        [Display(Name = "Course")]
        public long? CourseId { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }
    }
}
