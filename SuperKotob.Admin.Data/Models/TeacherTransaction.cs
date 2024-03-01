using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("TeacherTransaction")]
    public class TeacherTransaction : DataModel
    {
        

        [MaxLength]
        [Required]
        [Display(Name = "English Reason")]
        public string Reason { get; set; }

        [MaxLength]
        [Required]
        [Display(Name = "Arabic Reason")]
        public string ReasonLT { get; set; }

        [Display(Name = "Payment Method")]
        [Required]
        public long? PaymentMethodId { get; set; }

        [MaxLength]
         [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Amount")]
        [Required]
        public decimal? Amount { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate
        {
            get;

            set;
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
        [Display(Name = "Teacher")]
        [Required]
        public long? TeacherId { get; set; }

        public long? CourseId { get; set; }

        public long? TrackId { get;  set; }
        [Required]
        public long? CountryId { get;  set; }

        public string PromocodeNumber { get;  set; }

        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
        public Track Track { get; set; }

    }
}
