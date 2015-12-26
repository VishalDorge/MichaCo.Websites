using System;
using System.Linq;
using System.Web.Http;
using CacheManager.Core;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;

namespace Website
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new UnityContainer();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            var cacheConfig = ConfigurationBuilder.BuildConfiguration(settings =>
            {
                settings
                    .WithSystemRuntimeCacheHandle("inprocess");
                ////.WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10));
                ////settings.WithRedisBackPlate("redisConnection");
                ////settings.WithRedisCacheHandle("redisConnection", true);
            });

            container.RegisterType(
                typeof(ICacheManager<>),
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(
                    (c, t, n) => CacheFactory.FromConfiguration(t.GetGenericArguments()[0], cacheConfig)));
        }
    }
}