using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotApiGw.Models {
    public class RouteRule {
        public string ApiGwSuffix { get; set; }
        public string SpSchema { get; set; } = "http";
        public string SpHost { get; set; }
        public int SpPort { get; set; } = 80;
        public string SpSuffix { get; set; }
        public string HttpMethod { get; set; } = "Get,Post";
        public string SpKey { get; set; }
    }
    public class ObjectConverter {

        public static FileReRoute ConvertToFileReRoute(RouteRule routeRule) {
            var fileReRoute = new FileReRoute();
            fileReRoute.Key = routeRule.ApiGwSuffix;
            fileReRoute.UpstreamPathTemplate = $"/{routeRule.ApiGwSuffix}/{{everything}}";
            fileReRoute.UpstreamHttpMethod = new List<string>(routeRule.HttpMethod.Split(','));

            fileReRoute.DownstreamScheme = routeRule.SpSchema;
            fileReRoute.DownstreamHostAndPorts = new List<FileHostAndPort> {
                new FileHostAndPort { Host=routeRule.SpHost, Port=routeRule.SpPort } };
            fileReRoute.DownstreamPathTemplate = $"/{routeRule.SpSuffix}/{{everything}}";
            if (!string.IsNullOrWhiteSpace(routeRule.SpKey))
                fileReRoute.AuthenticationOptions = new FileAuthenticationOptions { AuthenticationProviderKey = routeRule.SpKey, AllowedScopes = new List<string>() };
            return fileReRoute;
        }
        public static RouteRule ConvertToRouteRule(FileReRoute fileRoute) {
            if (fileRoute == null)
                return null;
            return new RouteRule
            {
                ApiGwSuffix = fileRoute.UpstreamPathTemplate,
                HttpMethod = string.Join(",", fileRoute.UpstreamHttpMethod),
                SpHost = fileRoute.DownstreamHostAndPorts[0].Host,
                SpPort = fileRoute.DownstreamHostAndPorts[0].Port,
                SpSchema = fileRoute.DownstreamScheme,
                SpSuffix = fileRoute.DownstreamPathTemplate,
                SpKey = fileRoute.AuthenticationOptions?.AuthenticationProviderKey
            };
        }

    }
}
