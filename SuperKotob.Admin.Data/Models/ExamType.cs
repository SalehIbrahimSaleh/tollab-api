using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("ExamType")]
    public class ExamType : DataModel
    {
        public ExamType()
        {
            this.Exams = new List<Exam>();
        }

        
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [MaxLength(50)]
        [Display(Name = "Name LT")]
        public string NameLT { get; set; }

        public List<Exam> Exams { get; set; }
    }
}
