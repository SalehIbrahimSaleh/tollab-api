using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("SubCategory")]
    public class SubCategory : DataModel
    {
        public SubCategory()
        {
            this.Departments = new List<Department>();
        }




        [Display(Name = "SubCategory-Category")]
        public string SubCategoryCategory { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

        [Display(Name = "Category")]
        [Required]
        public long? CategoryId { get; set; }

        public Category Category { get; set; }

        public List<Department> Departments { get; set; }
    }
}
