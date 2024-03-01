using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("TeacherSubject")]
    public class TeacherSubject : DataModel
    {
         

        [Display(Name = "Teacher")]
        [Required]
        public long? TeacherId { get; set; }

        [Display(Name = "Subject")]
        [Required]
        public long? SubjectId { get; set; }

        public Teacher Teacher { get; set; }
        public Subject Subject  { get; set; }
    }
}
