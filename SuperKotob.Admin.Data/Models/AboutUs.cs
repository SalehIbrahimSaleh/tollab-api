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
    [Table("AboutUs")]
   public class AboutUs:DataModel
    {
        [Required]
        [Display(Name = "English Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Arabic Description")]
        public string DescriptionLT { get; set; }

    }
}
