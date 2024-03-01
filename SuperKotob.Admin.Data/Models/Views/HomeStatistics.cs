using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models.Views
{
    [Table("HomeStatistics")]
   public class HomeStatistics
    {
        public int Id { get; set; }
        public int? StudentCount { get; set; }
        public int? TeachersCount { get; set; }
        public int?     CoursesCount { get; set; }
        public decimal? TotalIncome { get; set; }
        public decimal? ProfitWithStudentCredit { get; set; }
        public decimal? StudentCredit { get; set; }
        public decimal? TotalProfit { get; set; }
        public decimal? InstractorIncome { get; set; }
        [NotMapped]
        public List<Country> Countries { get; set; }
    }
}
