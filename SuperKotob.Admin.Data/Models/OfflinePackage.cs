using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("OfflinePackage")]
    public class OfflinePackage : DataModel
    {
        public OfflinePackage()
        {
            this.StudentPackages = new List<StudentPackage>();
        }
        [Required]
        [Display(Name = "Package Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Package Start Date")]
        public DateTime? PackageStartDate { get; set; }
        [Display(Name = "Package End Date")]
        public DateTime? PackageEndDate { get; set; }
        [Display(Name = "Price")]
        public decimal? NewPrice { get; set; }
        public decimal? SkuPrice { get; set; }
        public string SkuNumber { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public List<StudentPackage> StudentPackages { get; set; }


    }
}
