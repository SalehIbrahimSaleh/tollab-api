using SuperKotob.Admin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Models
{
    public interface IDataModel
    {
        long Id { get; set; }

        RequestInputs RequestInputs { get; set; }
      //  DateTime? CreatedOn { get; set; }

       // Guid? CreatedBy { get; set; }

      //  DateTime? ModifiedOn { get; set; }

      //  Guid? ModifiedBy { get; set; }
    }
}
