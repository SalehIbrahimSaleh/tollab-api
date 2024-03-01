using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("SearchWord")]
    public class SearchWord : DataModel
    {
        

        [Display(Name = "Student")]
        [Required]
        public long? StudentId { get; set; }

        [Required]
        [Display(Name = "Word")]
        public string Word { get; set; }

        public Student Student { get; set; }
    }
}
