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
    [Table("LiveAttachment")]
    public class LiveAttachment : DataModel
    {
        [Required]
        [Display(Name = "File name")]
        public string Name { get; set; }
        [Display(Name = "Path")]
        public string Path { get; set; }
        [Required]
        [Display(Name="Live")]
        public long LiveId { get; set; }

        [Display(Name = "Order")]
        public int? OrderNumber { get; set; }
        public Live Live { get; set; }
    }
}
