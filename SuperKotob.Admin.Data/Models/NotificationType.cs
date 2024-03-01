using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("NotificationType")]
    public class NotificationType : DataModel
    {
        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

    }
}
