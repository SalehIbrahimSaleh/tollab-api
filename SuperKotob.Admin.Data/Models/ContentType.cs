using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("ContentType")]
    public class ContentType : DataModel
    {
        public ContentType()
        {
            this.Contents = new List<Content>();
        }


        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }


        public List<Content> Contents { get; set; }
    }
}
