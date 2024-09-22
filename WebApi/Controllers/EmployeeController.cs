using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using WebApi.ViewModel;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }


        [HttpPost]
        public IActionResult Add([FromForm] EmployeeViewModel employeeView)
                {
                    if (employeeView.Photo == null || employeeView.Photo.Length == 0)
                    {
                        return BadRequest("Nenhuma foto foi enviada.");
                    }

                    // Gera um nome único para o arquivo usando um GUID
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeView.Photo.FileName);
                    var filePath = Path.Combine("Storage", fileName);

                    try
                    {
                        // Cria a pasta Storage se ela não existir
                        if (!Directory.Exists("Storage"))
                        {
                            Directory.CreateDirectory("Storage");
                        }

                        // Salva o arquivo na pasta Storage
                        using Stream fileStream = new FileStream(filePath, FileMode.Create);
                        employeeView.Photo.CopyTo(fileStream);

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
                return NotFound(); // Retorna 404 se o funcionário não for encontrado
            }

            employee = _employeeRepository.UpdateAgeName(id, name,age);

            return Ok(employee);
        }


    }
}
