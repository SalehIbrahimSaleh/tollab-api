using SuperKotob.Admin.Utils.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SuperKotob.Admin.Data.TransferObjects;
using SuperKotob.Admin.Data.Models;

namespace SuperKotob.Admin.UseCases.Signups
{
    public class SignUpMapper : IEntityMapper
    {
        public void Map(IMapperConfigurationExpression cfg)
        {

            cfg.CreateMap<SignUp, SignUpDTO>();

            cfg.CreateMap<SignUpDTO, SignUp>()
                .ForMember(item => item.Id, opts => opts.Ignore())
                .ForMember(item => item.ConfirmationCode, opts => opts.Ignore())
                .ForMember(item => item.CreatedBy, opts => opts.Ignore())
                .ForMember(item => item.CreatedOn, opts => opts.Ignore())
                .ForMember(item => item.ModifiedBy, opts => opts.Ignore())
                .ForMember(item => item.ModifiedOn, opts => opts.Ignore());
        }
    }
}
