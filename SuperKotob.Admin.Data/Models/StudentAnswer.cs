using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentAnswer")]
    public class StudentAnswer : DataModel
    {
       
        [Display(Name = "Student Exam")]
        public long? StudentExamId { get; set; }
        [Display(Name = "Exam Question")]
        public long? ExamQuestionId { get; set; }
        [Display(Name = "Exam Question Type")]
        public long? ExamQuestionTypeId { get; set; }
        [Display(Name = "Exam Answer")]
        public long? ExamAnswerId { get; set; }
        [Display(Name = "Degree")]
        public double? Degree { get; set; }
        [MaxLength]
        [Display(Name = "Answer Text")]
        public string AnswerText { get; set; }
        [MaxLength]
        [Display(Name = "Teacher Correctance")]
        public string TeacherCorrectance { get; set; }
        [MaxLength]
        [Display(Name = "Voice Path")]
        public string VoicePath { get; set; }
        [Display(Name = "Is True")]
        public bool? IsTrue { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }
        [MaxLength]
        [Display(Name = "Pdf Answer Path")]
        public string PdfAnswerPath { get; set; }
        [NotMapped]
        public string NewPdfAnswerPath
        {
            get
            {
                if (!string.IsNullOrEmpty(PdfAnswerPath))
                {
                    return "http://tollab.com/dashboard-v2/ws/AnswerFiles/" + PdfAnswerPath;
                }
                else return "#";
            }
        }
        
        public StudentExam StudentExam { get; set; }
        public ExamQuestion ExamQuestion  { get; set; }
        public ExamAnswer ExamAnswer { get; set; }
    }
}
