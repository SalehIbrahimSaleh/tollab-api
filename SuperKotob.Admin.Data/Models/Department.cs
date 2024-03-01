using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Department")]
    public class Department : DataModel
    {
        public Department()
        {
            this.CourseDepartments = new List<CourseDepartment>();
            this.StudentDepartments = new List<StudentDepartment>();
            this.Subjects = new List<Subject>();
        }


        [Display(Name = "Department-SubCategory")]
        public string DepartmentSubCategory { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }


        [Required]
        [Display(Name = "Sub Category")]
        public long? SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }

        public List<CourseDepartment> CourseDepartments { get; set; }

        public List<StudentDepartment> StudentDepartments { get; set; }

        public List<Subject> Subjects { get; set; }
    }
}
