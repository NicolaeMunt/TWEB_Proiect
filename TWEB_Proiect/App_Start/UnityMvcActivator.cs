using System.Linq;
using System.Web.Mvc;
using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TWEB_Project.UnityMvcActivator), nameof(TWEB_Project.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(TWEB_Project.UnityMvcActivator), nameof(TWEB_Project.UnityMvcActivator.Shutdown))]

namespace TWEB_Project
{
    public static class UnityMvcActivator
    {
        public static void Start()
        {
            var container = UnityConfig.Container;
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void Shutdown()
        {
            var container = UnityConfig.Container;
            container.Dispose();
        }
    }
}