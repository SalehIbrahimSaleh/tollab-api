using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tollab.Admin.Web.Services.FileUpload
{
    interface IFileUploadService
    {
        Task<HttpStatusCode> UploadImage(long recordId, string table, string columnName, int imageType, HttpPostedFileBase image);
        Task<string> UploadFile(HttpPostedFileBase file);
    }
}
