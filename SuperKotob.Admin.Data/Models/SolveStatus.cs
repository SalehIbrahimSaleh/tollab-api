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
    [Table("SolveStatus")]
   public class SolveStatus:DataModel
    {
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [MaxLength(50)]
        [Display(Name = "Name LT")]
        public string NameLT { get; set; }
    }
}
