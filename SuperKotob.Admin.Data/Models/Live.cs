using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Core.Enums;

namespace Tollab.Admin.Data.Models
{
    [Table("Live")]
    public class Live : DataModel
    {
        public Live()
        {
            this.StudentLives = new List<StudentLive>();
        }
        [Required]
        [Display(Name = "Live name")]
        public string LiveName { get; set; }
        [Required]
        [Display(Name = "Teacher")]
        public long TeacherId { get; set; }

        [Display(Name = "Order")]
        public int? OrderNumber { get; set; }

        [Display(Name = "Live link Type")]
        public LiveLinkType LiveLinkType { get; set; }
        [Required]
        [Display(Name = "Current Cost")]
        public decimal CurrentPrice { get; set; }
        [Display(Name = "Old Cost")]
        public decimal? OldPrice { get; set; }
        [Display(Name = "SKU Number")]
        public string SKUNumber { get; set; }
        [Display(Name = "Current SKU Cost")]
        public decimal? CurrentSKUPrice { get; set; }
        [Display(Name = "Old SKU Cost")]
        public decimal? OldSKUPrice { get; set; }
        public string HostURL { get; set; }
        public string JoinURL { get; set; }
        public string MeetingPassword { get; set; }
        [Display(Name = "Duration")]
        public int Duration { get; set; }
        [Display(Name = "Live time")]
        public DateTime LiveDate { get; set; }
        [Display(Name = "Appearance time")]
        public DateTime LiveAppearanceDate { get; set; }
        [Display(Name = "Video")]
        public string VideoURL { get; set; }
        public string VideoURI { get; set; }
        [Display(Name = "Cover Image")]
        public string Image { get; set; }
        [Display(Name = "Is Show In Web")]
        public bool? IsShowInWeb { get; set; }
        [Display(Name = "Close Live")]
        public bool? IsActive { get; set; }
        [Required]
        [Display(Name = "Country")]
        public long CountryId { get; set; }
        [Display(Name = "Course")]
        public long? CourseId { get; set; }
        [Display(Name = "Track")]
        public long? TrackId { get; set; }

        public long? ZoomMeetingId { get; set; }

        [Display(Name = "Course Order")]
        [NotMapped]
        public int? CourseOrder { get; set; }

        public List<StudentLive> StudentLives { get; set; }

        public int? ZoomAccount { get; set; }
        public Teacher Teacher { get; set; }
        public Track Track { get; set; }
        public Course Course { get; set; }
        public Country Country { get; set; }
    }
}
