using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob
{
    public class ApiRequest<T>  where T : SuperKotob.Admin.Models.IDataModel
    {
        public T Model { get; set; }
    }
}
