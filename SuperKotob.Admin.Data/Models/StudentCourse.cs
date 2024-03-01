using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentCourse")]
    public class StudentCourse : DataModel
    {
        [Display(Name = "Student")]
        [Required]
        public long? StudentId { get; set; }

        [Display(Name = "Course")]
        [Required]
        public long? CourseId { get; set; }

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EnrollmentDate {
            get;

            set;
        }
        [NotMapped]
        public DateTime? EnrollmentDate2
        {
            get
            {
                if (EnrollmentDate != null)
                {
                    return EnrollmentDate.Value.AddHours(3);
                }
                return EnrollmentDate;
            }
        }
        [Display(Name = "Completion Percentage")]
        public int? CompletionPercentage { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }

        [NotMapped]
        public RequestInputs RequestInputs { get; set; }
    }
}
