using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("[Subject]")]
    public class Subject : DataModel
    {
        public Subject()
        {
            this.Tracks = new List<Track>();
        }


        [Display(Name = "Subject-Department")]
        public string SubjectDepartment { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

        [Display(Name = "Department")]
        [Required]
        public long? DepartmentId { get; set; }

      
        [Display(Name = "Image")]
        public string Image { get; set; }

        public Department Department { get; set; }

        public List<Track> Tracks { get; set; }
    }
}
