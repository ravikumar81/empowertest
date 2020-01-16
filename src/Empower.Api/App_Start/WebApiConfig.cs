using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;

/********************************************************
 * This file is auto-generated and will be overwritten. *
 * Do not make any changes to it.                       *
 ********************************************************/

namespace Empower.Api {

	/// <summary>
	/// Contains the Pure Romance's standard API registration method, which is attribute routing.
	/// </summary>
    public static class WebApiConfig {

        public static void Register(HttpConfiguration config) {			
			var settings = config.Formatters.JsonFormatter.SerializerSettings;
			var contractResolver = new CamelCasePropertyNamesContractResolver();			
			settings.ContractResolver = contractResolver;
            

            //Get allowed domains from appSettings
            string domains = System.Configuration.ConfigurationManager.AppSettings["AllowedDomains"];
            config.EnableCors(new EnableCorsAttribute(domains, "*", "*"));

            config.MapHttpAttributeRoutes();
        }

    }

}