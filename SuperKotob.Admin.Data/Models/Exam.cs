using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Exam")]
    public class Exam : DataModel
    {
        public Exam()
        {
            this.ExamQuestions = new List<ExamQuestion>();
            this.StudentExams = new List<StudentExam>();
        }

         
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Deadline Date")]
        public DateTime DeadlineDate { get; set; }
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }
        [Display(Name = "Duration")]
        public int Duration { get; set; }
        [Display(Name = "Exam Type")]
        public long ExamTypeId { get; set; }
        [Display(Name = "Course")]
        public long CourseId { get; set; }
    
        [Display(Name = "Publish")]
        public bool Publish { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Lock Course Content" )]
        public bool LockCourseContent { get; set; }


        public ExamType ExamType { get; set; }
        public Course Course { get; set; }
        public List<ExamQuestion> ExamQuestions { get; set; }
        public List<StudentExam> StudentExams { get; set; }
    }
}
