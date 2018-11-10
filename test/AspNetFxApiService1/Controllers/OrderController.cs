using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNetFxApiService1.Controllers {
    //[Route("api/[controller]")]
    public class OrderController : ApiController {
        [HttpGet]
        public string Get(int id) {
            return $"you get order {id} from asp.net web api services";
        }
    }
}
