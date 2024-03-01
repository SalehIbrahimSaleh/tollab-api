using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentTransaction")]
    public class StudentTransaction : DataModel
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

        [Required]
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

        [Display(Name = "Student")]
        [Required]
        public long? StudentId { get; set; }

        [Display(Name = "Coures")]

        public long? CourseId { get; set; }

        public long? TrackId { get; internal set; }

        public string PromocodeNumber { get; internal set; }

        public Student Student { get; set; }
        public Course  Course { get; set; }
        public Track Track { get; set; }
    }
}
