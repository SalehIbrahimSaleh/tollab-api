using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tollab.Admin.Data.Models.Views;

namespace Tollab.Admin.Data.Models
{
    [Table("Student")]
    public class Student : DataModel
    {
        public Student()
        {
          
        }


        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Parent Name 1")]
        public string ParentName { get; set; }

        [Display(Name = "Parent Name 2")]
        public string ParentName2 { get; set; }

        [Display(Name = "Parent Phone 1")]
        public string ParentPhone { get; set; }

        [Display(Name = "Parent Phone 2")]
        public string ParentPhone2 { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Key")]
        public string PhoneKey { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Gender")]
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

        [Required]
        [Display(Name = "Login Count")]
        public int? NumberCurrentLoginCount { get; set; }

        [Display(Name = "Max Login Count")]
        [Required]
        public int? NumberMaxLoginCount { get; set; }

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
        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }

        [Display(Name = "Identity")]
        public string IdentityId { get; set; }

        [Required]
        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        [Display(Name = "Vcode")]
        public int? Vcode { get; set; }

        [Display(Name = "Expiration V Code Date")]
        public DateTime? ExpirationVCodeDate { get; set; }

        [Display(Name = "Verified")]
        public bool? Verified { get; set; }

        public string PaymentLink { get; set; }
        public string PaymentKey { get; set; }

     


        public DateTime? LastSendDate { get; set; }

        [Display(Name = "Screenshoot Count")]
        public int? ScreenShootCount { get; set; }
        [Display(Name = "Last Taken Screenshoot Date")]
        public DateTime? LastTakenScreenshootDate { get; set; }

        public List<Favourite> Favourites { get; set; }

        public List<SearchWord> SearchWords { get; set; }

        public List<StudentContent> StudentContents { get; set; }

        public List<StudentCourse> StudentCourses { get; set; }

        public List<StudentPackage> StudentPackages { get; set; }
        public List<StudentLive> StudentLives { get; set; }
        public List<TrackSubscription> StudentTracks { get; set; }
        public List<StudentDepartment> StudentDepartments { get; set; }

        public List<StudentNotification> StudentNotifications { get; set; }

        public List<StudentTransaction> StudentTransactions { get; set; }

        public List<VideoQuestion> VideoQuestions { get; set; }
        [NotMapped]
        public Department  Department { get; set; }
        [NotMapped]
        public Subject  Subject { get; set; }
        [NotMapped]
        public StudentNotification StudentNotification  { get; set; }
        [NotMapped]
        public StudentTransaction StudentTransaction { get; set; }
        public List<StudentSubject> StudentSubjects { get; set; }
        public List<StudentPromoCode> StudentPromoCodes { get; set; }
        public List<StudentExam> StudentExams { get; set; }

        public Country Country { get; set; }

    }
}
