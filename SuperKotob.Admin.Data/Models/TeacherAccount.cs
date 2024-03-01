using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("TeacherAccount")]
    public class TeacherAccount : DataModel
    {
         

        [Display(Name = "Teacher")]
        [Required]
        public long? TeacherId { get; set; }

        [MaxLength]
        [Display(Name = "Account Number")]
        [Required]
        public string AccountNumber { get; set; }

        [MaxLength]
        [Display(Name = "Note")]
        [Required]
        public string Note { get; set; }

        public Teacher Teacher { get; set; }
    }
}
