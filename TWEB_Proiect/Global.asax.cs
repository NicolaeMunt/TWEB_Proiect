using BusinessLogic.Data;
using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace TWEB_Proiect
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Database initialization - add these lines
            System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<BusinessLogic.Data.ApplicationDbContext>());

            // Force database creation and schema application
            using (var context = new BusinessLogic.Data.ApplicationDbContext())
            {
                context.Database.Initialize(force: true);
                System.Diagnostics.Debug.WriteLine("Database initialization attempted");
            }
        }
    }
}