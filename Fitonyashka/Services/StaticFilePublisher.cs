namespace Fitonyashka.Services;

public interface IStaticFilePublisher
{
    void Publish(string sourceFilePath, string targetDirectoryPath);
    void Delete(string fileName, string targetDirectoryPath);
}

internal class StaticFilePublisher : IStaticFilePublisher
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public StaticFilePublisher(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }

    public void Publish(string sourceFilePath, string targetDirectoryPath) {
        string fileName = Path.GetFileName(sourceFilePath);
        string dirPath = Path.Combine(_webHostEnvironment.WebRootPath, targetDirectoryPath);
        if (!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
        string targetFilePath = Path.Combine(dirPath, fileName);
        File.Copy(sourceFilePath, targetFilePath, true);
    }

    public void Delete(string fileName, string targetDirectoryPath) {
        string targetFilePath = Path.Combine(_webHostEnvironment.WebRootPath, targetDirectoryPath,
            fileName);
        if (File.Exists(targetFilePath)) {
            File.Delete(targetFilePath);
        }
    }
}
