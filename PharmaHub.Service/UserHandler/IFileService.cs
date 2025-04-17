using Microsoft.AspNetCore.Http;

namespace PharmaHub.Service.UserHandler
{
    public interface IFileService
    {
        string UploadFile(IFormFile file, string destinationFolder);
    }
}
