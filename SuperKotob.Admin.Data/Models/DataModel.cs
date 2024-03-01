using SuperKotob.Admin.Core;
using SuperKotob.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Data.Models
{
    public class DataModel : IDataModel
    {
        [Column(Order =0)]
        public long Id { get; set; }
     

        RequestInputs IDataModel.RequestInputs
        {
            get ;set ;
        }

      //  public Guid? CreatedBy { get; set; }
            //  public DateTime? CreatedOn { get; set; }
            //  public DateTime? ModifiedOn { get; set; }
     //   public Guid? ModifiedBy { get; set; }


    }
}
