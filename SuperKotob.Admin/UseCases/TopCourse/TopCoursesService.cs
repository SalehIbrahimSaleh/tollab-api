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

namespace Tollab.Admin.UseCases.TopCourse
{
    public class TopCoursesService : BaseService<Data.Models.Views.TopCourses, Data.Models.Views.TopCourses>
    {
        TopCoursesRepository _topCoursesRepository;
        public TopCoursesService(TopCoursesRepository repository, IValidator<TopCourses> validator, IDataMapper mapper) : base(repository, validator, mapper)
        {
            _topCoursesRepository = new TopCoursesRepository();
        }
    }
}
