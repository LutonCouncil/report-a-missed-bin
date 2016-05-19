﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Waste_Bin_Collections.Classes.Filters.Authorization;

namespace Waste_Bin_Collections
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new XsrfAuthorizationAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
