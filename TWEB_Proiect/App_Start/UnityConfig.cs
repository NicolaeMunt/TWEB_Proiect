using System;
using Unity;
using Unity.Lifetime;

namespace TWEB_Proiect
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container =
            new Lazy<IUnityContainer>(() =>
            {
                var container = new UnityContainer();
                RegisterTypes(container);
                return container;
            });

        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            // Регистрируем ApplicationDbContext
            container.RegisterType<TWEB_Proiect.Data.ApplicationDbContext>(new ContainerControlledLifetimeManager());
        }
    }
}