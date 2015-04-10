using System;
using System.Linq;
using System.Web.Http;
using CacheManager.Core;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using Website.Models;

namespace Website
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var container = new UnityContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            var cache = CacheFactory.Build<Todo[]>("todos", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("inprocess");
            });

            container.RegisterInstance(cache);
        }
    }
}