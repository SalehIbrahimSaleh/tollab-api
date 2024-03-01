using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentContent")]
    public class StudentContent : DataModel
    {
        
        [Required]
        [Display(Name = "Content")]
        public long? ContentId { get; set; }

        [Required]
        [Display(Name = "Student")]
        public long? StudentId { get; set; }

        public Content Content { get; set; }

        public Student Student { get; set; }
    }
}
