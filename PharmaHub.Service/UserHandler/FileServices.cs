using Microsoft.AspNetCore.Http;

namespace PharmaHub.Service.UserHandler;
public class FileServices : IFileService
{
    public string UploadFile(IFormFile file, string destinationFolder, string expectedType)
    {
        var uniqueFileName = string.Empty;

        if (file != null && file.Length > 0)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var contentType = file.ContentType.ToLower();

            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };

            if (expectedType == "pdf")
            {
                var isPdf = (contentType == "application/pdf" && extension == ".pdf");
                if (!isPdf)
                    throw new InvalidOperationException("Only PDF files are allowed for this field.");
            }
            else if (expectedType == "image")
            {
                var isImage = contentType.StartsWith("image/") && allowedImageExtensions.Contains(extension);
                if (!isImage)
                    throw new InvalidOperationException("Only image files (JPG, JPEG, PNG) are allowed for this field.");
            }
            else
            {
                throw new ArgumentException("Invalid expectedType. Use 'pdf' or 'image'.");
            }

            var uploadsFolder = Path.Combine(@"./wwwroot/", destinationFolder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        return uniqueFileName;
    }
}


