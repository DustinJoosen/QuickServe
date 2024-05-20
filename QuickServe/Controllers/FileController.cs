using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServe.Services.Interfaces;

namespace QuickServe.Controllers
{
    [Route("files")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private IAppService _appService;
        private IFileService _fileService;
        private IMapper _mapper;

        public FileController(IAppService appService, IFileService fileService, IMapper mapper)
        {
            this._appService = appService;
            this._fileService = fileService;
            this._mapper = mapper;
        }


        [HttpGet]
        [Route("{appId:Guid}")]
        public IActionResult Get([FromRoute]Guid appId)
        {
            // Check wether app exists
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            // Return files.
            return Ok(this._fileService.GetAppFiles(app));
        }

        [HttpGet]
        [Route("{appId:Guid}/{fileId:Guid}")]
        public Stream? Get([FromRoute]Guid appId, [FromRoute]Guid fileId)
        {
            // Check wether app exists
            var app = this._appService.GetById(appId);
            if (app == null)
                return null;

            // Check wether file exists
            var file = this._fileService.GetById(fileId);
            if (file == null)
                return null;

            // Return open stream
            return this._fileService.GetStream(file);
        }

        [HttpPost]
        [Route("{appId:Guid}")]
        public IActionResult Upload([FromRoute]Guid appId, IFormFile formFile)
        {
            // Check wether app exists
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            // Upload the file
            string fileName = this._fileService.Upload(appId, formFile);

            // Save the file info.
            var file = this._fileService.Create(new Entities.File
            {
                AppId = appId,
                FileName = fileName
            });

            if (file == null)
                return BadRequest("Could not upload file");

            return Ok(file);
        }

        [HttpPut]
        [Route("{appId:Guid}/{fileId:Guid}")]
        public IActionResult Update([FromRoute]Guid appId, [FromRoute]Guid fileId, IFormFile formFile)
        {
            // Check wether app exists
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            // Check wether file exists.
            var file = this._fileService.GetById(fileId);
            if (file == null)
                return NotFound();

            // Delete the old file
            this._fileService.RemoveFile(file);

            // Upload the new file
            string newFileName = this._fileService.Upload(appId, formFile);

            // Update file info and save.
            if (file == null)
                return NotFound();

            file.FileName = newFileName;
            var succeeded = this._fileService.Update(fileId, file);

            if (!succeeded)
                return BadRequest("Could not update file");

            return Ok(file);
        }

        [HttpDelete]
        [Route("{appId:Guid}/{fileId:Guid}")]
        public IActionResult Delete([FromRoute]Guid appId, [FromRoute]Guid fileId)
        {
            // Check wether app exists.
            var app = this._appService.GetById(appId);
            if (app == null)
                return NotFound();

            // Check wether file exists.
            var file = this._fileService.GetById(fileId);
            if (file == null)
                return NotFound();

            // Delete the file itself.
            this._fileService.RemoveFile(file);

            // Delete the file entity.
            var succeeded = this._fileService.Delete(file);
            if (!succeeded)
                return BadRequest("Could not delete file");

            return Ok(file);
        }
    }
}