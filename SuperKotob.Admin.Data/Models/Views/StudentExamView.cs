using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models.Views
{
    [Table("StudentExamView")]
    public class StudentExamView:DataModel
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string ParentName2 { get; set; }
        public string ParentPhone { get; set; }
        public string ParentPhone2 { get; set; }
        public string ExamName { get; set; }
        public int? TotalScore { get; set; }
        public double? TotalDegree { get; set; }
        [NotMapped]
        public long? StudentId { get; set; }
        [NotMapped]
        public long? ExamId { get; set; }
        [NotMapped]
        public RequestInputs RequestInputs { get; set; }

    }
}
