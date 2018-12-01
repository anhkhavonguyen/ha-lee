using System.Threading.Tasks;
using Harvey.PIM.API.Filters;
using Harvey.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/appsettings")]
    [Authorize]
    public class AppSettingsController : ControllerBase
    {
        private readonly IAppSettingService _service;
        public AppSettingsController(IAppSettingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _service.GetAsync();
            return Ok(result);
        }

        [HttpGet("{key}")]
        public async Task<ActionResult> Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest("key is required");
            }
            var result = await _service.GetByKeyAsync(key);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AppSettingModel appSetting)
        {
            if (string.IsNullOrEmpty(appSetting.Key))
            {
                return BadRequest("key is required");
            }
            var result = await _service.AddAsync(appSetting);
            return CreatedAtAction("Add", result);
        }

        [HttpPut]
        [Route("{key}")]
        public async Task<ActionResult> Update(string key, [FromBody] AppSettingModel appSetting)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest("key is required.");
            }
            var result = await _service.UpdateAsync(appSetting);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("id is required.");
            }
            var result = _service.Delete(id);
            return Ok();
        }
    }
}