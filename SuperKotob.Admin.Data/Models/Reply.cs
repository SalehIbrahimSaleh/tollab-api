using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Reply")]
    public class Reply : DataModel
    {
        

        [Required]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Video Question  ")]
        public long? VideoQuestionId { get; set; }

        [Display(Name = "Creation Date")]
        [Required]
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
        [Display(Name = "Student")]
        public long? StudentId { get; set; }

        [Display(Name = "Teacher")]
        public long? TeacherId { get; set; }

        [Display(Name = "View My Account")]
        [Required]
        public bool? ViewMyAccount { get; set; }

        public Teacher Teacher { get; set; }
        public Student Student { get; set; }
        public VideoQuestion VideoQuestion { get; set; }
    }
}
