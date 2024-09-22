
namespace WebApi.Services
{
    public class FileStorageService : IFileStorageService
    {
        public string SaveFile(IFormFile file, string folder)
        {

            if (file == null || file.Length == 0)
            {
                throw new Exception("Nenhum arquivo foi enviado.");
            }

            // Gera um nome único para o arquivo
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folder, fileName);

            // Cria a pasta se não existir
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // Salva o arquivo
            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            return filePath; // Retorna o caminho salvo


        }
    }
}
