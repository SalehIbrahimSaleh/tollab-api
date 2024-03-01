using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("ContactUs")]
    public class ContactUs : DataModel
    {
        

        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }

        [Display(Name = "Reviewed")]
        [Required]
        public bool? Status { get; set; }

        [Display(Name = "Student")]
        public long? StudentId { get; set; }

        public virtual Student Student { get; set; }

        [Display(Name = "Teacher")]
        public long? TeacherId { get; set; }
        public Teacher Teacher { get; set; }

    }
}
