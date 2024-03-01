using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }
        public ApiResponse(T model, params string[] errors)
            : this(new List<T>() { model }, errors)
        {
        }
        public ApiResponse(IList<T> model, params string[] errors)
        {
            this.Model = model;
            Errors = new List<string>(errors);
        }
        public ApiResponse(T model, IList<string> errors)
        {
            this.Model = new List<T>() { model };
            Errors = errors;
        }
        public ApiResponse(IList<T> model, IList<string> errors)
        {
            this.Model = model;
            Errors = errors;
        }

        [IgnoreDataMember]
        public bool HasModel
        {
            get
            {
                return (Model != null && Model.Count > 0);
            }
        }
        [IgnoreDataMember]
        public bool HasErrors
        {
            get
            {
                return (Errors != null && Errors.Count > 0);
            }
        }
        public IList<T> Model { get; set; }
        public ApiMeta Metas { get; set; }
        public IList<string> Errors { get; set; }

        public void AddToErrors(IList<string> errors)
        {
            this.Errors = this.Errors == null
                    ? new List<string>()
                    : this.Errors;

            this.Errors = this.Errors.IsReadOnly
                ? new List<string>(this.Errors)
                : this.Errors;

            foreach (var err in errors)
                this.Errors.Add(err);
        }
    }
}
