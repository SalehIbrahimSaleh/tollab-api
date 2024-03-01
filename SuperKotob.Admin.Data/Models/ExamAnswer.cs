using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("ExamAnswer")]
    public class ExamAnswer : DataModel
    {
        public ExamAnswer()
        {
            this.StudentAnswers = new List<StudentAnswer>();
        }

       
        [Display(Name = "Exam Question")]
        public long? ExamQuestionId { get; set; }
        [MaxLength]
        [Display(Name = "Answer")]
        public string Answer { get; set; }
        [Display(Name = "Is True")]
        public bool? IsTrue { get; set; }

        public ExamQuestion ExamQuestion { get; set; }
        public List<StudentAnswer> StudentAnswers { get; set; }
    }
}
