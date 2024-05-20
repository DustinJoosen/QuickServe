using Microsoft.AspNetCore.Mvc;
using QuickServe.Services.Interfaces;

namespace QuickServe.Controllers
{
    [Route("data")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IAppService _appService;

        public DataController(IDataService dataService, IAppService appService)
        {
            this._dataService = dataService;
            this._appService = appService;
        }

        [HttpGet]
        [Route("{appId:Guid}")]
        public IActionResult Get([FromRoute]Guid appId)
        {
            // Check wether app exists
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            // Get file content.
            var content = this._dataService.Get(appId);
            if (content == null)
                return BadRequest("Could not get content of datafile");

            return Ok(content);
        }

        [HttpPut]
        [Route("{appId:Guid}")]
        public IActionResult Set([FromRoute] Guid appId, [FromBody]string contents)
        {
            // Check wether app exists
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            // Set file content.
            bool succeeded = this._dataService.Set(appId, contents);
            if (!succeeded)
                return BadRequest("Could not set data file");

            return Ok();
        }
    }
}
