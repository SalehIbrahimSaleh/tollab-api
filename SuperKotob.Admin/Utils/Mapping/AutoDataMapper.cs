using AutoMapper;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data;
using SuperKotob.Admin.Data.Models;
 using SuperKotob.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Mapping
{
    public class AutoDataMapper : IDataMapper
    {
        Dictionary<Type, Type> MappedTypesDictionary { get; set; }
        object syncObject = new object();

        public AutoDataMapper(IEnumerable<IEntityMapper> entityMappers)
        {
            Mapper.Initialize(cfg =>
            {

                foreach (var mapper in entityMappers)
                    mapper.Map(cfg);

                //cfg.CreateMap<CategoryItems, CategoryItemsDTO>();
                //cfg.CreateMap<Category, CategoryDTO>()
                //    .ForMember(item => item.Children, opt => opt.Ignore());


                cfg.CreateMap<PagingMeta, ApiMeta>();

                cfg.CreateMap<ApiMeta, PagingMeta>()
                    .ForMember(item => item.PageSize, opts => opts.Ignore())
                    .ForMember(item => item.Skip, opts => opts.Ignore());

                cfg.CreateMap(typeof(DataResponse<>), typeof(ApiResponse<>))
                    .ForMember("Model", opts => opts.MapFrom("Data"))
                    .ForMember("Errors", opts => opts.MapFrom("Errors"))
                    .ForMember("Metas", opts => opts.MapFrom("Paging")); 
               


                //cfg.CreateMap<OrderStatu, OrderStatusDTO>();
                //cfg.CreateMap<OrderStatusDTO, OrderStatu>(MemberList.Source);

                //cfg.CreateMap<IList<Category>, IList<CategoryDTO>>().ConvertUsing((source, destination) =>
                //{
                //    var roots = source.Where(item => item.ParentId == null);
                //    var rootsDTO = new List<CategoryDTO>();

                //    foreach (var r in roots)
                //    {
                //        var rootCategoryDTO = Mapper.Map<CategoryDTO>(r);
                //        rootCategoryDTO.Children = source
                //                                .Where(item => item.ParentId == r.Id)
                //                                .Select(item => Mapper.Map<CategoryDTO>(item))
                //                                .ToList();
                //        rootsDTO.Add(rootCategoryDTO);
                //    }
                //    return rootsDTO;
                //});





                //cfg.CreateMap<CustomerAddress, AddressDTO>()
                //    .ForMember(item => item.Lat, opts => opts.MapFrom(source => source.Latitude))
                //    .ForMember(item => item.Long, opts => opts.MapFrom(source => source.Longitude))
                //    .ForMember(item => item.Name, opts => opts.MapFrom(source => source.Address))
                //    .ForMember(item => item.ExtraInfo, opts => opts.MapFrom(source => source.ExtraInfo));


             

                //cfg.CreateMap<ShoppingList, ShoppingListDTO>()
                //.ForMember(item => item.Items, opts =>
                //    opts.MapFrom(x =>
                //        x.ShoppingListItems.Select(y => y.Item)
                //    )
                //);

                //cfg.CreateMap<ShoppingListDTO, ShoppingList>()
                //.ForMember(item => item.ActionId, opts => opts.Ignore())
                //.ForMember(item => item.CronExpression, opts => opts.Ignore())
                //.ForMember(item => item.Customer, opts => opts.Ignore())
                //.ForMember(item => item.CreatedBy, opts => opts.Ignore())
                //.ForMember(item => item.CreatedOn, opts => opts.Ignore())
                //.ForMember(item => item.ModifiedBy, opts => opts.Ignore())
                //.ForMember(item => item.ModifiedOn, opts => opts.Ignore())
                //.ForMember(item => item.ShoppingListAction, opts => opts.Ignore())
                //.ForMember(item => item.ShoppingListType, opts => opts.Ignore())
                //.ForMember(item => item.TypeId, opts => opts.Ignore())
                //.ForMember(item => item.CustomerAddress, opts => opts.Ignore())
                //.ForMember(item => item.CustomerCard, opts => opts.Ignore())
                //.ForMember(item => item.PromoCode, opts => opts.Ignore())
                //.ForMember(item => item.ShoppingListItems, opts =>
                //        opts.MapFrom(x => x.Items)
                //        );


                

                //cfg.CreateMap<Customer, CustomerDTO>()
                //    .ForMember(item => item.Addresses, opts => opts.MapFrom(source => source.CustomerAddresses))
                //    .ForMember(item => item.Cards, opts => opts.MapFrom(source => source.CustomerCards));

                //cfg.CreateMap<Customer, CustomerLiteDTO>();

                //cfg.CreateMap<CustomerDTO, Customer>()
                //    .ForMember(item => item.CreatedBy, opts => opts.Ignore())
                //    .ForMember(item => item.CreatedOn, opts => opts.Ignore())
                //    .ForMember(item => item.ModifiedBy, opts => opts.Ignore())
                //    .ForMember(item => item.ModifiedOn, opts => opts.Ignore())
                //    .ForMember(item => item.AllowNotifications, opts => opts.Ignore())
                //    .ForMember(item => item.CustomerAddresses, opts => opts.MapFrom(source => source.Addresses))
                //    .ForMember(item => item.CustomerCards, opts => opts.MapFrom(source => source.Cards))
                //    .ForMember(item => item.Orders, opts => opts.Ignore())
                //    .ForMember(item => item.PaymentCustomerId, opts => opts.Ignore())
                //    .ForMember(item => item.ShoppingLists, opts => opts.Ignore())
                //    .ForMember(item => item.SupportTickets, opts => opts.Ignore());


                //cfg.CreateMap<SupportTicketDTO, SupportTicket>(MemberList.Source);
                //cfg.CreateMap<SupportTicket, SupportTicketDTO>();

                //cfg.CreateMap<SupportTicketTypeDTO, SupportTicketType>()
                //    .ForMember(item => item.CreatedBy, opts => opts.Ignore())
                //    .ForMember(item => item.CreatedOn, opts => opts.Ignore())
                //    .ForMember(item => item.ModifiedBy, opts => opts.Ignore())
                //    .ForMember(item => item.ModifiedOn, opts => opts.Ignore())
                //    .ForMember(item => item.SupportTickets, opts => opts.Ignore());


               // cfg.CreateMap<SupportTicketType, SupportTicketTypeDTO>();
            });
            Mapper.AssertConfigurationIsValid();
        }

        public Type FindSourceTypeFor<TDestination>()
        {
            if (MappedTypesDictionary == null)
            {
                lock (syncObject)
                {
                    if (MappedTypesDictionary == null)
                    {
                        MappedTypesDictionary = new Dictionary<Type, Type>();
                        var typeMaps = Mapper.Configuration.GetAllTypeMaps();
                        foreach (var tm in typeMaps)
                        {
                            MappedTypesDictionary.Add(tm.DestinationType, tm.SourceType);
                        }
                    }
                }
            }

            Type sourceType;
            MappedTypesDictionary.TryGetValue(typeof(TDestination), out sourceType);
            return sourceType;
        }
        public TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}
