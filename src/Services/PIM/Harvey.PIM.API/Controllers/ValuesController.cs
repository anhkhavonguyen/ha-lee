using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/values")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public ActionResult<List<string>> Get()
        {
            _logger.LogInformation("Get Values");
            _logger.LogWarning("Test Log Warning");
            _logger.LogError("Test Log error");
            return new List<string>() { "value 1", "value 2" };
        }

        [HttpGet]
        [Route("authorized")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<string>> AuthorizedGet()
        {
            return new List<string>() { "authorized value 1", "authorized value 2" };
        }
    }
}
