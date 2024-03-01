using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("TermAndCondition")]
    public class TermAndCondition : DataModel
    { 

        [MaxLength]
        [Required]
        [Display(Name = "English Title")]
        public string Title { get; set; }

        [MaxLength]
        [Required]
        [Display(Name = "Arabic Title")]
        public string TitleLT { get; set; }

        [Display(Name = "English Discription")]
        [Required]
        public string Discription { get; set; }

        [Display(Name = "Arabic Discription")]
        [Required]
        public string DiscriptionLT { get; set; }

        [Display(Name = "Type")]
        [Required]
        public bool? Type { get; set; }
    }
}
