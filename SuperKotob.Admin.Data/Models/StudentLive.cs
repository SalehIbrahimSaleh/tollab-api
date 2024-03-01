using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentLive")]
    public class StudentLive : DataModel
    {
        [Display(Name = "Student")]
        [Required]
        public long? StudentId { get; set; }

        [Display(Name = "Live")]
        [Required]
        public long? LiveId { get; set; }

        
        public Student Student { get; set; }

        public Live Live { get; set; }

        [NotMapped]
        public RequestInputs RequestInputs { get; set; }
    }
}
