using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OcelotApiGw.Models;
using Ocelot.Configuration.Repository;
using Ocelot.Configuration.Setter;

namespace OcelotApiGw.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase {
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
        [HttpGet]
        public async Task<IEnumerable<RouteRule>> Get() {
            var repo = await _repo.Get();
            var fileRoutes = repo.Data.ReRoutes;
            List<RouteRule> routeRules = new List<RouteRule>();
            if (fileRoutes == null || fileRoutes.Count == 0)
                return routeRules;
            foreach (var fileRoute in fileRoutes)
                routeRules.Add(ObjectConverter.ConvertToRouteRule(fileRoute));
            return routeRules;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) {
            var repo = await _repo.Get();
            var fileRoute = repo.Data.ReRoutes.Where(r => r.Key == id).FirstOrDefault();
            if (fileRoute == null)
                return new BadRequestObjectResult(new {Code=1,Message="the rule is not exist." });
            RouteRule rule = ObjectConverter.ConvertToRouteRule(fileRoute);
            return new OkObjectResult(rule);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RouteRule routeRule) {
            try {
                var repo = await _repo.Get();
                var fileConfiguration = repo.Data;
                var reRoute = fileConfiguration.ReRoutes.Where(r => r.Key == routeRule.ApiGwSuffix).FirstOrDefault();
                if (reRoute == null) {
                    var fileReRoute = ObjectConverter.ConvertToFileReRoute(routeRule);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]RouteRule routeRule) {
            var repo = await _repo.Get();
            var fileConfiguration = repo.Data;
            var reRoute = fileConfiguration.ReRoutes.Where(r => r.Key == routeRule.ApiGwSuffix).FirstOrDefault();
            if (reRoute == null) 
                return new BadRequestObjectResult("The route rule is not exist.");

            var newRoute = ObjectConverter.ConvertToFileReRoute(routeRule);
            reRoute.UpstreamPathTemplate = newRoute.UpstreamPathTemplate;
            reRoute.DownstreamScheme = newRoute.DownstreamScheme;
            reRoute.DownstreamHostAndPorts = newRoute.DownstreamHostAndPorts;
            reRoute.DownstreamPathTemplate = newRoute.DownstreamPathTemplate;
            reRoute.AuthenticationOptions = newRoute.AuthenticationOptions;
            return new OkObjectResult("OK");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) {
            var repo = await _repo.Get();
            var fileRoute = repo.Data.ReRoutes.Where(r => r.Key == id).FirstOrDefault();
            if (fileRoute != null)
                repo.Data.ReRoutes.Remove(fileRoute);
            return new OkObjectResult("OK");
        }
    }

}