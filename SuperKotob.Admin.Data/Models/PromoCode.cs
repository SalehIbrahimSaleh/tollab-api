using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("PromoCode")]
    public class PromoCode : DataModel
    {
        [Display(Name = "Pattern")]
        public string Pattern { get; set; }

        [Required]
        [Display(Name = "Count")]
        public long? Count { get; set; }

        public long? UsedCount { get; set; }

        [Display(Name = "Promo Code Text")]
        public string PromoCodeText { get; set; }

        [Required]
        [Display(Name = "Promo Code Value")]
        public decimal? PromoCodeValue { get; set; }

        [Display(Name = "Expiration Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpirationDate {
            get;

            set;
        }
        [NotMapped]
        public DateTime? ExpirationDate2
        {
            get
            {
                if (ExpirationDate != null)
                {
                    return ExpirationDate.Value.AddHours(3);
                }
                return ExpirationDate;
            }
        }

        //pattern

        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        [Display(Name = "Section")]
        public long? SectionId { get; set; }

         [Display(Name = "Category")]
        public long? CategoryId { get; set; }

        [Display(Name = "Sub Category")]
        public long? SubCategoryId { get; set; }

         
        [Display(Name = "Department")]
        public long? DepartmentId { get; set; }

        public DateTime? CreationDate { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }


    }
}
