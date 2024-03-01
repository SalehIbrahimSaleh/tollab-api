using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Country")]
    public class Country : DataModel
    {
        public Country()
        {
            this.Sections = new List<Section>();
        }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }


        [Display(Name = "Flag")]
        public string Flag { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Length Phone Numebr")]
        public int? LengthPhoneNumebr { get; set; }

        [Required]
        [Display(Name = "English Currency")]
        public string Currency { get; set; }

        [Required]
        [Display(Name = "Arabic Currency")]
        public string CurrencyLT { get; set; }

        [Required]
        [Display(Name = "Country label")]
        public string CountryCode { get; set; }

        public string WhatsappNumber { get; set; }
        
        public List<Section> Sections { get; set; }
    }
}
