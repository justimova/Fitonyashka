namespace Fytonyashka.Services;

public interface IFileService
{
    Task<string> UploadFileAsync(string dirName, int id, IFormFile? file);
    Task DeleteFileAsync(string dirName, string fileName);
}

internal class FileService : IFileService
{
    public async Task<string> UploadFileAsync(string dirName, int id, IFormFile? file) {
        string filePath = null;
        if (file != null && file.Length > 0) {
            string baseDirectory = Directory.GetCurrentDirectory();
            string dataDirectory = Path.Combine(baseDirectory, "Data");
            var dirPath = Path.Combine(dataDirectory, dirName);
            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }

            var fileName = $"{id}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
            filePath = Path.Combine(dirPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }
        }
        return filePath;
    }

    public async Task DeleteFileAsync(string dirName, string fileName) {
        string baseDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(baseDirectory, "Data", dirName, fileName);
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }
    }
}