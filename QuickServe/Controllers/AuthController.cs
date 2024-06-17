using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServe.Dtos;
using QuickServe.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace QuickServe.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;
        public AuthController(IApiKeyService apiKeyService)
        {
            this._apiKeyService = apiKeyService;
        }

        [HttpPost]
        [Route("generate")]
        public IActionResult GenerateApiKey([FromBody]GenerateAPIKeyDto generateAPIKeyDto)
        {
            // Generate a hash.
            using var sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(generateAPIKeyDto.MasterPassword));
            string hashedPw = BitConverter.ToString(bytes).Replace("-", "").ToLower();

            // Compare the hash.
            string masterPw = "44d56713759f583c21e06423019c52897b7e1ebeea2522085ab56059fab65883";
            if (hashedPw != masterPw)
            {
                return Unauthorized();
            }

            var apiKey = this._apiKeyService.GenerateApiKey();
            return Ok($"Your API-Key is: {apiKey}. Copy it and store it somewhere safe. It won't be shown again");
        }
    }
}
