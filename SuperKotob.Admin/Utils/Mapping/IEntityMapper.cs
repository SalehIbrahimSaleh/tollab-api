using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Mapping
{
    public interface IEntityMapper
    {
        void Map(IMapperConfigurationExpression cfg);
    }
}
