﻿using System.Web;
using System.Web.Mvc;

namespace TWEB_Proiect
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
