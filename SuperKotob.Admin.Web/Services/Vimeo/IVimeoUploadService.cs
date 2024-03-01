using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Tollab.Admin.Web.Services.Vimeo
{
    public interface IVimeoUploadService
    {
        Task<(HttpStatusCode, JObject)> InitiateUpload(string name, double videoLength, string videoPassword, string vimeoToken);
        Task TranferVideo(string videoId, string vimeoToken);
        Task<(bool success, string URL, string videoURI, string errorDetails)> TryUpload(HttpPostedFileBase postedFile, string name, double videoLength, string videoPassword, string vimeoToken);
        Task DeleteFromViemoAsync(string VideoUri, string vimeoToken);
    }
}