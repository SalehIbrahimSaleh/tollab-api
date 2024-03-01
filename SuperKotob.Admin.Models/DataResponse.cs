using SuperKotob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core
{

    public class DataResponse<T>
    {
        public DataResponse()
        {
        }
        public DataResponse(params string[] errors)
        {
            this.Errors = errors;
        }
        public DataResponse(T data, params string[] errors)
            : this(new List<T>() { data }, errors)
        {
        }
        public DataResponse(IList<T> data, params string[] errors)
        {
            this.Data = data;
            this.Errors = errors;
        }

        public Dictionary<string, IList<LookupItem>> Lookups = new Dictionary<string, IList<LookupItem>>();

        public bool HasData
        {
            get
            {
                return Data != null && Data.Count > 0 && Data[0] != null;
            }
        }
        public bool HasErrors
        {
            get
            {
                return Errors != null
                    && Errors.Count > 0;
            }
        }
        public IList<T> Data { get; set; }
        public IList<string> Errors { get; set; }
        public PagingMeta Paging { get; set; }
        public RequestInputs RequestInputs { get; set; }

        public DataResponse<T> AddError(string error)
        {
            this.Errors = this.Errors == null ? new List<string>() : this.Errors;
            this.Errors.Add(error);
            return this;
        }
    }
}
