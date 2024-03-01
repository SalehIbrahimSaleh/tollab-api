using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Course")]
    public class Course : DataModel
    {
        public Course()
        {
            this.CourseDepartments = new List<CourseDepartment>();
            this.Favourites = new List<Favourite>();
            this.Groups = new List<Group>();
            this.Offers = new List<Offer>();
            this.StudentCourses = new List<StudentCourse>();
            this.TrackPromotionCourses = new List<TrackPromotionCourse>();
        }
        [Display(Name = "Course-Track")]
        public string CourseTrack { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }
        [Display(Name = "ProviderType")]
        public string ProviderType { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

        [Required]
        [Display(Name = "Track")]
        public long? TrackId { get; set; }

        [Required]
        [Display(Name = "Current Cost")]
        public decimal? CurrentCost { get; set; }

        [Required]
        [Display(Name = "Previous Cost")]
        public decimal? PreviousCost { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime? CreationDate
        {
            get;set;
        }

        [NotMapped]
         [Display(Name = "Creation Date")]
        public DateTime? CreationDate2
        {
            get
            {
                if (CreationDate!=null)
                {
                    return CreationDate.Value.AddHours(3);
                }
                return CreationDate;
            }
         }
        [Display(Name = "Views Count Per Student For Course")]
        public int? AllowedShowTimes { get; set; }

        [Required]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [MaxLength]
        [Display(Name = "Full Description")]
        public string FullDescription { get; set; }

        [Display(Name = "Subscription Count")]
        public long? SubscriptionCount { get; set; }

        [MaxLength]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [MaxLength]
        [Display(Name = "Intro Video")]
        public string IntroVideo { get; set; }

        [Required]
        [Display(Name = "Course Status")]
        public long? CourseStatusId { get; set; }


        [Display(Name = "Order Number")]
        public int? OrderNumber { get; set; }

        [Display(Name = "Show Water Mark")]
        public bool? ShowWaterMark { get; set; }
        [Display(Name = "Need Parent")]
        public bool? NeedParent { get; set; }
        [Required]
        [Display(Name = "Is Must Solve Exam")]
        public bool AnswerExam { get; set; }
        public bool? IsShowInWeb { get; set; }

        public bool? IsAllowToDownload { get; set; }
        

        [Display(Name = "SKU Number")]
        public string SKUNumber { get; set; }
        [Display(Name = "SKU Price")]
        public decimal? SKUPrice { get; set; }
        [Display(Name = "Old SKU Price")]
        public decimal? OldSKUPrice { get; set; }

        public string  IntroVideoUri { get; set; }
        public List<CourseDepartment> CourseDepartments { get; set; }

        public List<Favourite> Favourites { get; set; }

        public List<Group> Groups { get; set; }

        public List<Offer> Offers { get; set; }
        public List<TrackPromotionCourse> TrackPromotionCourses { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        public virtual Track  Track { get; set; }
        public virtual CourseStatus CourseStatus { get; set; }
        public string AlbumUri { get; set; }
        public string CourseCode { get; set; }

        // public int? SubscriptionDuration { get; set; }
    }
}
