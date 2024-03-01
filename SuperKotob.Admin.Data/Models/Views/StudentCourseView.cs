using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models.Views
{
   [Table("StudentCourseView")]
   public class StudentCourseView:DataModel
    {
        public string Name { get; set; }
        public string StudentNumber { get; set; }
        public string ParentName    { get; set; }
        public string ParentName2   { get; set; }
        public string ParentPhone   { get; set; }
        public string ParentPhone2  { get; set; }
        public string CourseName   { get; set; }
        public string CourseTrack  { get; set; }
        public string ExamSummary  { get; set; }
        [NotMapped]
        [Display(Name="Course")]
        public long? CourseId { get; set; }
        [NotMapped]
        [Display(Name = "Student")]
        public long? StudentId { get; set; }
        [NotMapped]
        public RequestInputs RequestInputs { get; set; }

    }
}
