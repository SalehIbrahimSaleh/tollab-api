using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("SystemTransaction")]
    public class SystemTransaction : DataModel
    {
    

        [MaxLength]
        [Required]
        [Display(Name = "Reason")]
        public string Reason { get; set; }

        [Display(Name = "Amount")]
        [Required]
        public decimal? Amount { get; set; }

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

    }
}
