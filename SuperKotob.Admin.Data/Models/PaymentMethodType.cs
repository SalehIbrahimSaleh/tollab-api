using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models
{
    [Table("dbo.PaymentMethodType")]
    public class PaymentMethodType : DataModel
    {
        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }


        [Required]
        [Display(Name = "Country")]
        public long CountryId { get; set; }

        [Required]
        [Display(Name = "The Factor")]
        public double? TheFactor { get; set; }

        public Country Country { get; set; }

        [Required]
        [Display(Name = "In AppPurchase")]
        public bool? InAppPurchase { get; set; }

    }

}
