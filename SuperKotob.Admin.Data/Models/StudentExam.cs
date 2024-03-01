using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentExam")]
    public class StudentExam : DataModel
    {
        public StudentExam()
        {
            this.StudentAnswers = new List<StudentAnswer>();
        }

        
        [Display(Name = "Student")]
        public long? StudentId { get; set; }
        [Display(Name = "Exam")]
        public long? ExamId { get; set; }
        [Display(Name = "Solve Status")]
        public long? SolveStatusId { get; set; }
        [Display(Name = "Total Score")]
        public int? TotalScore { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Teacher Assistant")]
        public long? TeacherAssistantId { get; set; }

        public Student Student { get; set; }
        public Exam Exam { get; set; }
        public SolveStatus SolveStatus { get; set; }
        public TeacherAssistant TeacherAssistant { get; set; }
        public List<StudentAnswer> StudentAnswers { get; set; }
    }
}
