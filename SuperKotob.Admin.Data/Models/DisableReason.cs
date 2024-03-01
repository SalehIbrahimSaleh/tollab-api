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
    [Table("DisableReason")]
   public class DisableReason:DataModel
    {

        [Display(Name ="Student")]
        public long StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Display(Name = "Reason")]
        public string Reason { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

    }
}
