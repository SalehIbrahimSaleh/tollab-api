using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models.Views
{
    [Table("TopCourses")]
   public class TopCourses:DataModel
    {
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public long CountryId { get; set; }
        public Country Country { get; set; }
    }
}
