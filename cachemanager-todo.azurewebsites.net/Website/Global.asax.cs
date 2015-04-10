using System;
using System.Linq;
using System.Web.Http;
using CacheManager.Core;
using CacheManager.Core.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;

namespace Website
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var container = new UnityContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            var cache = CacheFactory.Build("todos", settings =>
            {
                settings
                    .WithSystemRuntimeCacheHandle("inprocess")
                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                    .And
                    .WithRedisBackPlate("redis")
                    .WithRedisCacheHandle("redis", true)
                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(20));
            });

            container.RegisterInstance(cache);
        }
    }
}