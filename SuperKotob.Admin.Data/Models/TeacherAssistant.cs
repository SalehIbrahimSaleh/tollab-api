using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("TeacherAssistant")]
    public class TeacherAssistant : DataModel
    {
        public TeacherAssistant()
        {
            this.StudentExams = new List<StudentExam>();
        }

        
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Teacher")]
        public long? TeacherId { get; set; }

        [Display(Name = "Enabled")]
        [Required]
        public bool? Enabled { get; set; }
        [NotMapped]
        public string IsEnabled
        {
            get
            {
                if (Enabled == null)
                    return "Not Set";
                if (Enabled == true)
                    return "Enabled";
                if (Enabled == false)
                    return "Disabled";

                return "Nothing";
            }
        }

        [Display(Name = "Identity Id")]
        public string IdentityId { get; set; }

        [NotMapped]
        [Display(Name = "password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public Teacher Teacher { get; set; }
        public List<StudentExam> StudentExams { get; set; }
        public DateTime? RegisterationDate { get; set; }
    }
}
