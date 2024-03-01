using AutoMapper;
using StructureMap;
using SuperKotob.Admin.Data.Models;
 using SuperKotob.Admin.Web.Utils.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SuperKotob.Admin.Web
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController
            GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            Container container = null;
            try
            {
                container = GetContainer(requestContext, controllerType);
                if ((requestContext == null) || (controllerType == null))
                    return null;

                return (Controller)container.GetInstance(controllerType);
            }
            catch (StructureMapException ex)
            {
                System.Diagnostics.Debug.WriteLine(container.WhatDoIHave());
                throw;

                //throw new Exception(container.WhatDoIHave());
            }
        }
        static Container _Container;

        private Container GetContainer(RequestContext requestContext, Type controllerType)
        {
            if ((requestContext == null) || (controllerType == null))
            {
                if (_Container == null)
                {
                    _Container = new Container(x =>
                    {
                        x.AddRegistry<AdminRegistry>();
                    });
                }
                return _Container;
            }

            return new Container(x =>
            {
                x.AddRegistry<AdminRegistry>();
            });
        }
    }



    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());


            Mapper.Initialize(cfg =>
            {

                //cfg.CreateMap<OrderItem, ItemDTO>()
                //    .ForMember(item => item.Id, opts => opts.MapFrom(source => source.ItemId))
                //    .ForMember(item => item.OldPrice, opts => opts.Ignore())
                //    .ForMember(item => item.IsInCustomerMonthlyBag, opts => opts.Ignore())
                //    .ForMember(item => item.Size, opts => opts.MapFrom(source => source.Item.ItemSize.Name))
                //    .ForMember(item => item.Quantity, opts => opts.MapFrom(source => source.Quantity))
                //    .ForMember(item => item.Name, opts => opts.MapFrom(source => source.Item.Name))
                //    .ForMember(item => item.Description, opts => opts.MapFrom(source => source.Item.Description))
                //    .ForMember(item => item.ImageUrl, opts => opts.MapFrom(source => source.Item.DefaultImage.Url));

                //cfg.CreateMap<ItemDTO, OrderItem>()
                //    .ForMember(item => item.ItemId, opts => opts.MapFrom(source => source.Id))
                //    .ForMember(item => item.Quantity, opts => opts.MapFrom(source => source.Quantity))
                //    .ForMember(item => item.Price, opts => opts.MapFrom(source => source.Price))
                //    .ForAllOtherMembers(opts => opts.Ignore());

                //cfg.CreateMap<Order, OrderDTO>()
                //     .ForMember(item => item.TotalPrice, opts => opts.MapFrom(source => source.Total))
                //     .ForMember(item => item.Date, opts => opts.MapFrom(source => source.CreatedOn))
                //     .ForMember(item => item.Customer, opts => opts.MapFrom(source => source.Customer))
                //     .ForMember(item => item.StatusText, opts => opts.MapFrom(source => source.OrderStatus.Name))
                //     .ForMember(item => item.Items, opts => opts.MapFrom(source => source.OrderItems))
                //     .ForAllOtherMembers(opts => opts.Ignore());

                //cfg.CreateMap<OrderDTO, Order>()
                //    .ForMember(item => item.CustomerId, opts => opts.MapFrom(source => source.CustomerId))
                //    .ForMember(item => item.CustomerCardId, opts => opts.MapFrom(source => source.Card.Id))
                //    .ForMember(item => item.CustomerAddressId, opts => opts.MapFrom(source => source.Address.Id))
                //    .ForMember(item => item.OrderItems, opts => opts.MapFrom(source => source.Items))
                //    .ForMember(item => item.Total, opts => opts.MapFrom(source => source.TotalPrice))
                //    .ForMember(item => item.SubTotal, opts => opts.MapFrom(source => source.SubTotal))
                //    .ForMember(item => item.StatusId, opts => opts.MapFrom(source => source.StatusId))
                //    .ForAllOtherMembers(opts => opts.Ignore());


            });
        }
    }
}
