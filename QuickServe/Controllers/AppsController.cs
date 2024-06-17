using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServe.Attributes;
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

        private readonly IAppService _appService;
        private readonly IApiKeyService _apiKeyService;
        private readonly IFileService _fileService;
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public AppsController(IAppService appService, IFileService fileService, 
            IDataService dataService, IMapper mapper, IApiKeyService apiKeyService)
        {
            this._appService = appService;
            this._fileService = fileService;
            this._mapper = mapper;
            this._dataService = dataService;
            this._apiKeyService = apiKeyService;
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
        [Route("{appId:Guid}")]
        [ApiKeyRequired]
        public IActionResult GetById([FromRoute]Guid appId)
        {
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            return Ok(app);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody]AppCreationDto appCreation)
        {
            var app = this._mapper.Map<App>(appCreation);
            var createdApp = this._appService.Create(app);

            if (createdApp == null)
                return BadRequest("Could not create app");

            return Ok(createdApp);
        }


        [HttpPut]
        [Route("{appId:Guid}")]
        [ApiKeyRequired]
        public IActionResult Update([FromRoute]Guid appId, [FromBody]AppUpdateDto appUpdate)
        {
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            // Update the fields.
            app.Display = appUpdate.Display;
            app.Description = appUpdate.Description;
            app.RequiresAuthorization = appUpdate.RequiresAuthorization;

            var succeeded = this._appService.Update(appId, app);

            if (!succeeded)
                return BadRequest("Could not delete app");

            return Ok(app);
        }

        [HttpDelete]
        [Route("{appId:Guid}")]
        [ApiKeyRequired]
        public IActionResult Delete([FromRoute]Guid appId)
        {
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            var succeeded = this._appService.Delete(app);

            // Remove all files associated with the app.
            this._fileService.RemoveAppFiles(app);
            this._dataService.DeleteDataFile(app);

            if (!succeeded)
                return BadRequest("Could not delete app");

            return Ok(app);
        }
    }
}
