using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof($safeprojectname$.Startup))]

namespace $safeprojectname$
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(config =>
            {
                BootStrap.Configure(app, config);
            });
        }
    }

    // can be used in context with IClassFixture<> to startup a web server between tests
    // if so desired
    public class SelfHostStartup
    {
        public void Configuration(IAppBuilder app)
        {
            BootStrap.Configure(app, new HttpConfiguration());
        }
    }

    public static class BootStrap
    {
        public static void Configure(IAppBuilder app, HttpConfiguration config)
        {
            // simplify content negotiation to just always use json
            var jsonFormatter = new JsonMediaTypeFormatter();
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            // this makes models in the form "CapFriendlyBackEnd" translate over the wire
            // to a browser as "capFriendlyBackEnd" (for javascripty sensibilities)
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // point to HSI Vault server
            var authoritySource = ConfigurationManager.AppSettings["HsiVaultAuthorizationServer"];

            var options = new IdentityServerBearerTokenAuthenticationOptions()
            {
                Authority = authoritySource,
                ValidationMode = ValidationMode.Both
            };
            // enable the use of [ResourceAuthorize("Read", "Ping")] style authorization headers
            app.UseResourceAuthorization(new AuthorizationManager());
            // turn on identity server authorization for this endpoint
            app.UseIdentityServerBearerTokenAuthentication(options);

            // if the api is hosted at a different domain than the requesting application, an allowable
            // origin policy can be set here
            var cors = new EnableCorsAttribute(ConfigurationManager.AppSettings["EnabledCorsLocations"], "*", "*");
            config.EnableCors(cors);

            // new up the composition root and replace the controller activator to engage your code
            var controllerActivator = new CompositionRoot();
            config.Services.Replace(typeof(IHttpControllerActivator), controllerActivator);

            // establish all the routes defined in route prefix and route attributes
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }

    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;
        }

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            return new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
        }
    }
}
