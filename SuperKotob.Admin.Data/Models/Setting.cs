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
    [Table("Setting")]

    public class Setting:DataModel
    {
        [Required]
        [Display(Name = "Key")]
        public string SettingKey { get; set; }
        [Required]
        [Display(Name = "Value")]
        public string SettingValue { get; set; }

    }
}
