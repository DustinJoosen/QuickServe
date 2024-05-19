using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServe.Dtos;
using QuickServe.Entities;
using QuickServe.Services;
using QuickServe.Services.Interfaces;

namespace QuickServe.Controllers
{
    [Route("apps")]
    [ApiController]
    public class AppsController : ControllerBase
    {

        private IAppService _appService;
        private IMapper _mapper;
        public AppsController(IAppService appService, IMapper mapper)
        {
            this._appService = appService;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var apps = this._appService.GetAll();
            if (apps == null)
                return NotFound();

            return Ok(apps);
        }

        [HttpGet]
        [Route("{uuid:Guid}")]
        public IActionResult GetById([FromRoute]Guid uuid)
        {
            var app = this._appService.GetById(uuid);
            if (app == null)
                return NotFound();

            return Ok(app);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody]AppCreationDto appCreation)
        {
            var app = this._mapper.Map<App>(appCreation);
            var succeeded = this._appService.Create(app);

            if (!succeeded)
                return BadRequest("Could not create app");

            return Ok(app);
        }

        [HttpPut]
        [Route("{uuid:Guid}")]
        public IActionResult Update([FromRoute]Guid uuid, [FromBody]AppUpdateDto appUpdate)
        {
            var app = this._appService.GetById(uuid);
            if (app == null)
                return NotFound();

            // Update the fields.
            app.Display = appUpdate.Display;
            app.Description = appUpdate.Description;
            app.RequiresAuthorization = appUpdate.RequiresAuthorization;

            var succeeded = this._appService.Update(uuid, app);

            if (!succeeded)
                return BadRequest("Could not delete app");

            return Ok(app);
        }

        [HttpDelete]
        [Route("{uuid:Guid}")]
        public IActionResult Delete([FromRoute]Guid uuid)
        {
            var app = this._appService.GetById(uuid);
            if (app == null)
                return NotFound();

            var succeeded = this._appService.Delete(app);

            if (!succeeded)
                return BadRequest("Could not delete app");

            return Ok(app);
        }
    }
}
