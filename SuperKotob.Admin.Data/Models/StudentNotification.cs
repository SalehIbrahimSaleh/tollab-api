using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentNotification")]
    public class StudentNotification : DataModel
    {
        [MaxLength]
        [Required]
        [Display(Name = "English Title")]
        public string Title { get; set; }
        [MaxLength]
        [Required]
        [Display(Name = "Arabic Title")]
        public string TitleLT { get; set; }

        [Display(Name = "Notification Type")]
        public long? NotificationTypeId { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate
        {
            get;set;
        }
        [NotMapped]
        public DateTime? CreationDate2
        {
            get
            {
                if (CreationDate != null)
                {
                    return CreationDate.Value.AddHours(3);
                }
                return CreationDate;
            }

        }

        [Display(Name = "Notification To Id")]
        public long? NotificationToId { get; set; }

        [Display(Name = "Student")]
        public long? StudentId { get; set; }

        [Display(Name = "Reference")]
        public long? ReferenceId { get; set; }
        public virtual Reference Reference { get; set; }
        public Student Student { get; set; }

        [NotMapped]
        public long? CountryId { get; set; }
        [NotMapped]
        public long?  CategoryId { get; set; }
    }
}
