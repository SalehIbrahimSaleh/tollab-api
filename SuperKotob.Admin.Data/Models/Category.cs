using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Category")]
    public class Category : DataModel
    {
        public Category()
        {
            this.SubCategories = new List<SubCategory>();
        }

        [Display(Name = "Category-Section")]
        public string CategorySection { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

        [Required]
        [Display(Name = "Section")]
        public long? SectionId { get; set; }

        public Section Section { get; set; }

        public List<SubCategory> SubCategories { get; set; }
    }
}
