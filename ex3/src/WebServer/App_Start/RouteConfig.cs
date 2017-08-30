using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebServer
{
	public class RouteConfig
    {
		/// <summary>
		/// Registers the routes.
		/// </summary>
		/// <param name="routes">The routes.</param>
		public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
	        routes.MapMvcAttributeRoutes();
		}
    }
}

