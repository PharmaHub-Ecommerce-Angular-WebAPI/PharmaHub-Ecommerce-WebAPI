using Microsoft.AspNetCore.Http;

namespace PharmaHub.Service.UserHandler;

public class FileServices : IFileService
{
    public string UploadFile(IFormFile file, string destinationFolder)
    {
        var uniqueFileName = string.Empty;

        if (file != null && file.Length > 0)
        {
            // Check if the file is a PDF
            if (file.ContentType != "application/pdf" && Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                throw new InvalidOperationException("Only PDF files are allowed.");
            }

            var uploadsFolder = Path.Combine(@"./wwwroot/", destinationFolder);

            // Ensure the folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);  // Ensures unique name
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        return uniqueFileName;
    }
}
