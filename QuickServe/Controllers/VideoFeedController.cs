using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace QuickServe.Controllers
{
    public class CameraHub : Hub
    {
        public async Task SendFrame(byte[] frameBytes)
        {
            await this.Clients.All.SendAsync("ReceiveFrame", frameBytes);
        }
    }


    [Route("feed")]
    [ApiController]
    public class VideoFeedController : ControllerBase
    {
        private static byte[] LATEST_FRAME_BYTES;
        private readonly IHubContext<CameraHub> _hubContext;

        public VideoFeedController(IHubContext<CameraHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        [HttpPost]
        [Route("stream")]
        public async Task<IActionResult> ReceiveFrame()
        {
            try
            {
                using MemoryStream memoryStream = new();

                await this.Request.Body.CopyToAsync(memoryStream);
                var frameBytes = memoryStream.ToArray();

                LATEST_FRAME_BYTES = frameBytes;

                // Send the frame to all connected clients
                await this._hubContext.Clients.All.SendAsync("ReceiveFrame", frameBytes);

                return this.Ok("Frame received successfully");
            }
            catch (Exception ex)
            {
                return this.BadRequest($"Error processing the frame: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("frame")]
        public IActionResult GetFrame()
        {
            try
            {
                // If no frames exist, assign it the unavailable.jpg file
                LATEST_FRAME_BYTES ??= System.IO.File.ReadAllBytes("data/unavailable.jpg");

                // Return the image as a response
                return this.File(LATEST_FRAME_BYTES, "image/jpeg");
            }
            catch (Exception ex)
            {
                return this.BadRequest($"Error returning frame: {ex.Message}");
            }
        }


    }
}
