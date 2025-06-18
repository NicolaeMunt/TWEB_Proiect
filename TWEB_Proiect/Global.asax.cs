using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TWEB_Proiect.Data;           // Правильное пространство имен
using TWEB_Proiect.Domain.Entities; // Если нужны сущности

namespace TWEB_Proiect
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
               Database.SetInitializer<ApplicationDbContext>(null);
               using (var context = new TWEB_Proiect.Data.ApplicationDbContext())
            {
                context.Database.CreateIfNotExists();
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Если есть инициализация базы данных:
            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }
    }
}