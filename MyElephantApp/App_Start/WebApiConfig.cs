using Amazon.XRay.Recorder.Handlers.AspNet.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MyElephantApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Add the message handler to HttpConfiguration
            config.MessageHandlers.Add(new TracingMessageHandler("MyElephant-WebAPI"));
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
