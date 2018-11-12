using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Configuration.Setter;

namespace OcelotApiGw.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IFileConfigurationRepository _repo;
        private readonly IFileConfigurationSetter _setter;
        private readonly IServiceProvider _provider;

        public ConfigController(IFileConfigurationRepository repo, IFileConfigurationSetter setter, IServiceProvider provider) {
            _repo = repo;
            _setter = setter;
            _provider = provider;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var response = await _repo.Get();

            if (response.IsError) {
                return new BadRequestObjectResult(response.Errors);
            }

            return new OkObjectResult(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FileConfiguration fileConfiguration) {
            try {
                var response = await _setter.Set(fileConfiguration);

                if (response.IsError) {
                    return new BadRequestObjectResult(response.Errors);
                }

                return new OkObjectResult(fileConfiguration);
            }
            catch (Exception e) {
                return new BadRequestObjectResult($"{e.Message}:{e.StackTrace}");
            }
        }
    }
}