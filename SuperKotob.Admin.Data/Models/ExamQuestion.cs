using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("ExamQuestion")]
    public class ExamQuestion : DataModel
    {
        public ExamQuestion()
        {
            this.ExamAnswers = new List<ExamAnswer>();
        }

        
        [MaxLength]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [MaxLength]
        [Display(Name = "Image")]
        public string Image { get; set; }
        [Display(Name = "Order Number")]
        public int? OrderNumber { get; set; }
        [Display(Name = "Exam Question Type Id")]
        public long? ExamQuestionTypeId { get; set; }
        [Display(Name = "Exam Id")]
        public long? ExamId { get; set; }
        [Display(Name = "Degree")]
        public double? Degree { get; set; }
        [MaxLength]
        [Display(Name = "File Path")]
        public string FilePath { get; set; }

        [NotMapped]
        public string NewPath
        {
            get
            {
                if (!string.IsNullOrEmpty(FilePath))
                {
                    return "http://tollab.com/ws/ExamFiles/" + FilePath;
                }
                else return "#";
            }
        }

        public ExamQuestionType ExamQuestionType { get; set; }
        public Exam Exam { get; set; }
        public List<ExamAnswer> ExamAnswers { get; set; }
    }
}
