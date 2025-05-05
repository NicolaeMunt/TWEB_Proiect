using System.Linq;
using System.Web.Mvc;
using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TWEB_Proiect.UnityMvcActivator), nameof(TWEB_Proiect.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(TWEB_Proiect.UnityMvcActivator), nameof(TWEB_Proiect.UnityMvcActivator.Shutdown))]

namespace TWEB_Proiect
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