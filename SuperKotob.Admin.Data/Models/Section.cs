using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Section")]
    public class Section : DataModel
    {
        public Section()
        {
            this.Categories = new List<Category>();
        }


      
        [Display(Name = "Section-Country")]
        public string SectionCountry { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        
        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

        [Display(Name = "Country")]
        [Required]
        public long? CountryId { get; set; }

        [MaxLength]
        [Display(Name = "Image")]
        public string Image { get; set; }

        public Country Country { get; set; }

        public List<Category> Categories { get; set; }
    }
}
