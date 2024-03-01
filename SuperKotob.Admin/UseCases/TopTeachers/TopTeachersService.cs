using SuperKotob.Admin.Core.Utils.Validation;
using SuperKotob.Admin.Data;
using SuperKotob.Admin.UseCases.Base;
using SuperKotob.Admin.Utils.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models.Views;

namespace Tollab.Admin.UseCases.TopTeachers
{
    public class TopTeachersService : BaseService<Data.Models.Views.TopTeachers, Data.Models.Views.TopTeachers>
    {
        TopTeachersRepository _topTeachersRepository;
        public TopTeachersService(TopTeachersRepository repository, IValidator<Data.Models.Views.TopTeachers> validator, IDataMapper mapper) : base(repository, validator, mapper)
        {
            _topTeachersRepository = repository;
        }
    }
}
