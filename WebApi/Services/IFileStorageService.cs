namespace WebApi.Services
{
    public interface IFileStorageService
    {
        string SaveFile(IFormFile file, string folder);
    }
}
