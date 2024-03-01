using SuperKotob.Admin.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Role")]
    public class Role : DataModel
    {
   
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [MaxLength(50)]
        [Display(Name = "Name LT")]
        public string NameLT { get; set; }
    }
}
