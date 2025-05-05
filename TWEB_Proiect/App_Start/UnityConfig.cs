using BusinessLogic.Data;
using BusinessLogic.Repositories;
using BusinessLogic.Services;
using Domain.Interfaces;
using System;
using Unity;
using Unity.Lifetime;

namespace TWEB_Proiect
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            // Database context
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());

            // Repositories
            container.RegisterType<IUserRepository, UserRepository>();

            // Services
            container.RegisterType<IUserService, UserService>();
        }
    }
}