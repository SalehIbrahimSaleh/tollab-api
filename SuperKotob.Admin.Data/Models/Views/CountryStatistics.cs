using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models.Views
{
    [Table("CountryStatistics")]
   public class CountryStatistics
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public int? StudentCount { get; set; }
        public int? StudentCountInThisMonth { get; set; }
        public int? TeachersCount { get; set; }
        public int? TeacherCountInThisMonth { get; set; }
        public int? CourseCount { get; set; }
        public int? CourseCountInThisMonth { get; set; }
        public decimal? TotalIncome { get; set; }
        public decimal? InstractorIncome { get; set; }
        public decimal? TotalIncomeInThisMonth { get; set; }
        public decimal? InstractorIncomeInThisMonth { get; set; }
        public decimal? ProfitWithStudentCredit { get; set; }
        public decimal? StudentCredit { get; set; }
        public decimal? TotalProfit { get; set; }

    }
}
