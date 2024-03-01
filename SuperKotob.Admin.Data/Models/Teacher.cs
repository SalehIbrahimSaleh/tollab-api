using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Teacher")]
    public class Teacher : DataModel
    {
        public Teacher()
        {
            this.Replies = new List<Reply>();
            this.TeacherAccounts = new List<TeacherAccount>();
            this.TeacherNotifications = new List<TeacherNotification>();
            this.TeacherTransactions = new List<TeacherTransaction>();
            this.Tracks = new List<Track>();
        }



        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [MaxLength]
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MaxLength]
        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [MaxLength]
        [Required]
        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public bool? Gender { get; set; }

        [NotMapped]
        public string IsGender
        {
            get
            {
                if (Gender == null)
                    return "Not Set";
                if (Gender == true)
                    return "Male";
                if (Gender == false)
                    return "Female";

                return "Nothing";
            }
        }

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

        [MaxLength]
        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Country")]
        [Required]
        public long? CountryId { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Registeration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RegisterationDate { get; set; }

        [MaxLength]
        [Display(Name = "Face Book Link")]
        public string FaceBookLink { get; set; }

        [Required]
        [Display(Name = "Taken Percentage")]
        public int? TakenPercentage { get; set; }

        [Display(Name = "Live Taken Percentage")]
        public int? LiveTakenPercentage { get; set; }

        [MaxLength]
        [Display(Name = "Twitter Link")]
        public string TwitterLink { get; set; }

        [MaxLength]
        [Display(Name = "Instagram")]
        public string Instagram { get; set; }

        [MaxLength]
        [Display(Name = "Identity Id")]
        public string IdentityId { get; set; }

        [Display(Name = "Can add live")]
        public bool CanAddLive { get; set; }

        public List<Reply> Replies { get; set; }

        public List<TeacherAccount> TeacherAccounts { get; set; }

        public List<TeacherNotification> TeacherNotifications { get; set; }

        public List<TeacherTransaction> TeacherTransactions { get; set; }

        public List<Track> Tracks { get; set; }

        [NotMapped]
        [Display(Name = "password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public Country Country { get; set; }
    }
}
