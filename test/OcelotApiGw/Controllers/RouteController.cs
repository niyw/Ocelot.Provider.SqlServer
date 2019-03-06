using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Configuration.Setter;

namespace OcelotApiGw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IFileConfigurationRepository _repo;
        private readonly IFileConfigurationSetter _setter;
        private readonly IServiceProvider _provider;
        private readonly ILogger<RouteController> _logger;
        public RouteController(IFileConfigurationRepository repo, IFileConfigurationSetter setter, IServiceProvider provider, ILogger<RouteController> logger) {
            _repo = repo;
            _setter = setter;
            _provider = provider;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RouteRule routeRule) {
            try {
                _logger.Log(LogLevel.Information, "Start Post Method For RerouteController...");

                var repo = await _repo.Get();
                var fileConfiguration = repo.Data;
                var reRoute = fileConfiguration.ReRoutes.Where(r => r.Key == routeRule.ApiGwSuffix).FirstOrDefault();
                if (reRoute == null) {
                    var fileReRoute = ConvertToFileReRoute(routeRule);                    
                    fileConfiguration.ReRoutes.Add(fileReRoute);

                    var response = await _setter.Set(fileConfiguration);

                    if (response.IsError) {
                        return new BadRequestObjectResult(response.Errors);
                    }
                }
                else {
                    return new BadRequestObjectResult("Add FileReRoute Failed, Key is duplicate.");
                }

                return new OkObjectResult("OK");
            }
            catch (Exception e) {
                _logger.Log(LogLevel.Error, "Post Method Happen Error For RerouteController.");
                return new BadRequestObjectResult($"{e.Message}:{e.StackTrace}");
            }
        }
        private static FileReRoute ConvertToFileReRoute(RouteRule routeRule) {
            var fileReRoute = new FileReRoute();
            fileReRoute.UpstreamPathTemplate = $"/{routeRule.ApiGwSuffix}/{{everything}}";
            fileReRoute.UpstreamHttpMethod = new List<string>(routeRule.HttpMethod.Split(','));

            fileReRoute.DownstreamScheme = routeRule.SpSchema;
            fileReRoute.DownstreamHostAndPorts = new List<FileHostAndPort> {
                new FileHostAndPort { Host=routeRule.SpHost, Port=routeRule.SpPort } };
            fileReRoute.DownstreamPathTemplate = $"/{routeRule.SpSuffix}/{{everything}}";
            if(!string.IsNullOrWhiteSpace( routeRule.SpKey))
                fileReRoute.AuthenticationOptions = new FileAuthenticationOptions { AuthenticationProviderKey=routeRule.SpKey, AllowedScopes=new List<string>() };
            return fileReRoute;
        }
    }
    public class RouteRule {
        public string ApiGwSuffix { get; set; }
        public string SpSchema { get; set; }
        public string SpHost { get; set; }
        public int SpPort { get; set; }
        public string SpSuffix { get; set; }
        public string HttpMethod { get; set; } = "Get,Post";
        public string SpKey { get; set; }
    }
}