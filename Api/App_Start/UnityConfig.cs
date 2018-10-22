using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using Contract.Interfaces;

namespace Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            
            container.RegisterType<IStatusService, Business.Implementation.StatusService>();
            container.RegisterType<ISubscriptionsService, Business.Implementation.SubscriptionsService>();
            container.RegisterType<IDimensionsService, Business.Implementation.DimensionsService>();
            

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}