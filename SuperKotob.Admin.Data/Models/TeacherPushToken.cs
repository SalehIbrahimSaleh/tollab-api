using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models
{
    [Table("TeacherPushToken")]
    public class TeacherPushToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public string OS { get; set; }
        public long TeacherId { get; set; }

    }
}
