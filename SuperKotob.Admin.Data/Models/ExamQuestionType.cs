using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("ExamQuestionType")]
    public class ExamQuestionType : DataModel
    {
        public ExamQuestionType()
        {
            this.ExamQuestions = new List<ExamQuestion>();
        }

         
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [MaxLength(50)]
        [Display(Name = "Name LT")]
        public string NameLT { get; set; }

        public List<ExamQuestion> ExamQuestions { get; set; }
    }
}
