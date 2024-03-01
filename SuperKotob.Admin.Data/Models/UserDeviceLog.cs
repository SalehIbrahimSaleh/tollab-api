using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models
{
    [Table("UserDeviceLog")]
    public class UserDeviceLog
    {
        public long Id { get; set; }
        public string AppVersion { get; set; }
        public string DeviceName { get; set; }
        public string OS { get; set; }
        public DateTime? CreationDate { get; set; }
        public string IPAddress { get; set; }
        public long? StudentId { get; set; }
        public bool? Status { get; set; }
    }
}
