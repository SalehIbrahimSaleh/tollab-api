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
    [Table("StudentSubject")]
   public class StudentSubject:DataModel
    {

        public long SubjectId { get; set; }
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

        [Display(Name = "StudentId")]
        [Required]
        public long? StudentId { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        
        public Department Department { get; set; }
       
        public Student  Student { get; set; }
        public Subject Subject { get; set; }
    }
}
