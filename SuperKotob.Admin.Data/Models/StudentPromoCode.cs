using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("StudentPromoCode")]
    public class StudentPromoCode : DataModel
    {
         
        [Required]
        [Display(Name = "Student Id")]
        public long? StudentId { get; set; }

        [Required]
        [Display(Name = "Promo Code Id")]
        public long? PromoCodeId { get; set; }
        public Student Student { get; set; }
        public PromoCode  PromoCode { get; set; }

    }
}
