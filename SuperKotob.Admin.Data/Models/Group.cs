using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("[Group]")]
    public class Group : DataModel
    {
        public Group()
        {
            this.Contents = new List<Content>();
        }

        [Display(Name = "Group-Course")]
        public string GroupCourse { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

        [Required]
        [Display(Name = "Course")]
        public long? CourseId { get; set; }
       
        [Required]
        [Display(Name = "Order Number")]
        public int? OrderNumber { get; set; }
        
        public Course Course { get; set; }

        public List<Content> Contents { get; set; }
    }
}
