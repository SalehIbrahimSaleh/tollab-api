using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Web.Services.Live
{
    public interface ILiveToCourseConverterService
    {
        Task ConvertToCourse(Data.Models.Live live);
    }
}
