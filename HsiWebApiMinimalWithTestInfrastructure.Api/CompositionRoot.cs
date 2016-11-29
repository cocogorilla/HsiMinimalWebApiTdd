using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using $safeprojectname$.Controllers;
using $ext_safeprojectname$.Core.Models;
using $ext_safeprojectname$.Infrastructure.Repositories;

namespace $safeprojectname$
{
    public class CompositionRoot : IHttpControllerActivator
    {
        public IHttpController Create(
            HttpRequestMessage httpRequest,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            if (controllerType == typeof(PingController))
            {
                var config = new AppConfig()
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["OneSourceConnectionString"].ConnectionString
                };
                var pingRepo = new PingRepository(config);
                return new PingController(pingRepo);
            }

            throw new ArgumentException(
                "Unknown Controller type: " + controllerType,
                nameof(controllerType));
        }
    }
}