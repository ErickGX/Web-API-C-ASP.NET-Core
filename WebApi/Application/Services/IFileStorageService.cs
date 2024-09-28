namespace WebApi.Application.Services
{
    public interface IFileStorageService
    {
        string SaveFile(IFormFile file, string folder);
    }
}
