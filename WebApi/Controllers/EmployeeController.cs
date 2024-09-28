using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Services;
using WebApi.Application.ViewModel;
using WebApi.Domain.Model;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, IFileStorageService fileStorageService, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }




        //Rota para cadastrar usuario com foto no banco de dados
        [Authorize]
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



        //Rota para acessar e visualizar a foto do perfil do usuario com base no seu ID
        [Authorize]
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



        //Retorna todos os registros do banco de dados
        [Authorize]
        [HttpGet]
        public IActionResult Get() 
        {
            var employees = _employeeRepository.Get();
            
            return Ok(employees);

        }


        //rota do get com Paginação de dados
        [Authorize]
        [HttpGet("paginated/{pageNumber}/{pageQuantity}")]
        public IActionResult Get(int pageNumber, int pageQuantity) 
        {
            if (pageNumber <= 0 || pageQuantity <= 0)
            {
                return BadRequest("Page number e page quantity tem que ser maiores que Zero.");
            }


            _logger.Log(LogLevel.Error, "Erro na consulta");

            var employees = _employeeRepository.Get(pageNumber, pageQuantity);

            _logger.LogInformation("teste");
            return Ok(employees);
        }




        //Rota para deletar usuario com base no ID
        [Authorize]
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


        //Rota para atualizar apenas alguns parametros do usuario utilizando Patch
        [Authorize]
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
