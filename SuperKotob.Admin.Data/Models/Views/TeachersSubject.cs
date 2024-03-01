using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models.Views
{
  [Table("TeachersSubject")]
  public  class TeachersSubject:DataModel
    {

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

        [Display(Name = "Teacher")]
        [Required]
        public long? TeacherId { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }


        public Department Department { get; set; }

        public Teacher  Teacher { get; set; }
    }
}
