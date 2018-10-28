using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CarRental.Api
{
	/// <summary>
	/// 
	/// </summary>
    public static class WebApiConfig
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

  			config.Routes.MapHttpRoute(
				name: "swagger_root",
				routeTemplate: "",
				defaults: null,
				constraints: null,
				handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));


			config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}"
            );
        }
    }
}
