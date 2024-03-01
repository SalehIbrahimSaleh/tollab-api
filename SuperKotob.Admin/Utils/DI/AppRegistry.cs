using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Utils.Mapping;
using SuperKotob.Admin.Utils.Logging;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SuperKotob.Admin.Payment;
using SuperKotob.Admin.Utils.Payment;
using SuperKotob.Admin.Data;
using SuperKotob.Admin.Data.Repositories;
using SuperKotob.Admin.Data.Models;
using SuperKotob.Admin.UseCases.Base;
 //using SuperKotob.Admin.UseCases.Orders;
using SuperKotob.Admin.Utils.Sms;
//using SuperKotob.Admin.BusinessServices;
using SuperKotob.Admin.Core.Utils.Validation;
using System.Reflection;
//using SuperKotob.Admin.UseCases.CustomerCards;
using SuperKotob.Admin.UseCases;
using SuperKotob.Admin.Utils.Http;
 //using SuperKotob.UseCases.PushTokens;
using SuperMatjar.WebProtector.Core;
using SuperMatjar.WebProtector;

namespace SuperKotob.Admin.Utils.DI
{

    public class AppRegistry : Registry
    {
        public AppRegistry()
        {


            For<IProtectedUserRepository>().Use<AspnetIdentityUserRepository>().Singleton();
            For<ILogger>().Use<SeriLogger>().Singleton();

            For<IHttpClient>().Use<SuperKotobHttpClient>().Singleton();
 

            For(typeof(IBusinessService<,>)).Use(typeof(BaseService<,>));
            For(typeof(IRepository<>)).Use(typeof(BaseRepository<>));
            For(typeof(IValidator<>)).Use(typeof(NoValidator<>));


            this.Scan(x =>
            {
                x.TheCallingAssembly();
                x.AddAllTypesOf<IEntityMapper>();
            });

       

            LoadCustomTypesAndReplaceWithDefaultOne(typeof(IRepository<>), typeof(BaseRepository<>));
            LoadCustomTypesAndReplaceWithDefaultOne(typeof(IBusinessService<,>), typeof(BaseService<,>));

            For<ILogger>().Use<SeriLogger>();
            For<IDataMapper>().Use<AutoDataMapper>().Singleton();
         //   For<IPushManager>().Use<PushManager>().Singleton();
            For<ISmsService>().Use<NexmoSmsService>();
      //      For<IPaymentService>().Use<PayfortPaymentService>().Singleton();
            For<IAppConfigurations>().Use<AppConfigurations>().Singleton();
            For<IConfigurationService>().Use<ConfigurationService>().Singleton();
            For<IDependencyContainerFactory>().Use<DependencyContainerFactory>().Singleton();
            For<IDependencyContainerScope>().Use<StaticDependencyContainerScope>().Singleton();
            For<IConfigurationValueAccessorFactory>().Use<ConfigurationValueAccessorFactory>().Singleton();

        }

        private void LoadCustomTypesAndReplaceWithDefaultOne(Type interfaceType, Type baseType)
        {
            var types = Assembly.GetExecutingAssembly()
               .GetTypes()
               .Where(item => item.BaseType != null)
               .Where(item => item.BaseType.IsGenericType)
               .Where(item => item.BaseType.GetGenericTypeDefinition() != null)
               .Where(item => item.BaseType.GetGenericTypeDefinition().Name == baseType.Name);

            var repoInterfaceName = interfaceType.Name;
            var repoBaseName = baseType.Name;

            foreach (var t in types)
            {
                var foundType = t.FindInterfaces((x, y) =>
                {
                    return x.Name == repoInterfaceName;
                }, null).FirstOrDefault();

                if (foundType == null)
                    continue;

                For(foundType).Use(t);
            }
            //For(typeof(IRepository<CategoryItems>)).Use(typeof(CategoryItemsRepository));
            //For(typeof(IRepository<ShoppingList>)).Use(typeof(ShoppingListRepository));
            //For(typeof(IRepository<Order>)).Use(typeof(OrderRepository));
            //For(typeof(IRepository<SignUp>)).Use(typeof(SignUpsRepository));
            //For(typeof(IRepository<Item>)).Use(typeof(ItemRepository));
            //For(typeof(IRepository<Customer>)).Use(typeof(CustomerRepository));
            //For(typeof(IRepository<CustomerCard>)).Use(typeof(CustomerCardRepository));
        }
    }
}
