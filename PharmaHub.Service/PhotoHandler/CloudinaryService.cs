using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PharmaHub.Service.PhotoHandler
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(string cloudName, string Apikey,string apiSecret )
        {
            var account = new Account(cloudName, Apikey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription($"{Guid.NewGuid().ToString()}_{file.FileName}", stream),
                Folder = "uploads"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString(); // Return image URL
        }
    }
}
/*
 * builder.Services.Configure<CloudinaryConfig>(builder.Configuration.GetSection("Cloudinary")); in cs
 * 
 * 
 * [ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly CloudinaryService _cloudinaryService;

    public ImageController(IConfiguration config)
    {
        _cloudinaryService = new CloudinaryService(config);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid image");

 public ImageController(IConfiguration config)
    {
        var cloudName = config["Cloudinary:CloudName"];
        var apiKey = config["Cloudinary:ApiKey"];
        var apiSecret = config["Cloudinary:ApiSecret"];

        _cloudinaryService = new CloudinaryService(cloudName, apiKey, apiSecret);
    }        return Ok(new { imageUrl = url });
    }
}
*/
