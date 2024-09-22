using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using WebApi.Services;
using WebApi.ViewModel;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFileStorageService _fileStorageService;

        public EmployeeController(IEmployeeRepository employeeRepository, IFileStorageService fileStorageService)
        {

            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(employeeRepository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _fileStorageService = fileStorageService;
        }


        [HttpPost]
        public IActionResult Add([FromForm] EmployeeViewModel employeeView)
        {


                    try
                    {
                        // Salva a foto e retorna o caminho
                        var filePath = _fileStorageService.SaveFile(employeeView.Photo, "Storage");

                        // Cria um novo Employee e salva no banco de dados
                        var employee = new Employee(employeeView.Name, employeeView.Age, filePath);
                        _employeeRepository.Add(employee);

                        // Retorna o objeto Employee criado com sucesso
                        return Ok(employee);
                    }
                    catch (Exception ex)
                    {
                        // Se algo der errado, retorna uma resposta com a mensagem de erro
                        return StatusCode(500, $"Erro ao salvar o arquivo: {ex.Message}");
                    }
        }



        [HttpPost]
        [Route("{id}/download")]
        public IActionResult DownloadPhoto(int id)
        {
            var employee = _employeeRepository.Get(id);

            if (employee == null)
            {
                return NotFound();
            }

            var dataBytes = System.IO.File.ReadAllBytes(employee.photo);

            return File(dataBytes, "image/jpg");

        }



        [HttpGet]
        public IActionResult Get() 
        {
            var employees = _employeeRepository.Get();
            
            return Ok(employees);

        }


        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id) {

            var employee = _employeeRepository.Get(id);

            if (employee == null)
            {
                return NotFound(); // Retorna 404 se não encontrado
            }

            employee = _employeeRepository.Delete(id);


            return NoContent();
        }


        [HttpPatch]
        [Route("{id}/age/name")]
        public IActionResult UpdateNameAge(int id, string name, int age) {


            var employee = _employeeRepository.Get(id);

            if (employee == null)
            {
                return NotFound(); //  Retorna 404 se o funcionário não for encontrado
            }

            employee = _employeeRepository.UpdateAgeName(id, name,age);

            return Ok(employee); // Retorna o objeto atualizado ao cliente
        }


    }
}
