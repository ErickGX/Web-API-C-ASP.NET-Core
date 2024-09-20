﻿using Microsoft.AspNetCore.Mvc;
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

            var filePath = Path.Combine("Storage", employeeView.Photo.FileName);

            using Stream filestream = new FileStream(filePath, FileMode.Create);
            employeeView.Photo.CopyTo(filestream);

            var employee = new Employee(employeeView.Name, employeeView.Age, filePath);

            _employeeRepository.Add(employee);


            return Ok();

        }

        [HttpPost]
        [Route("{id}/download")]
        public IActionResult DownloadPhoto(int id)
        {
            var employee = _employeeRepository.Get(id);

            var dataBytes = System.IO.File.ReadAllBytes(employee.photo);

            return File(dataBytes, "image/jpg");

        }



        [HttpGet]
        public IActionResult Get() 
        {
            var employees = _employeeRepository.Get();
            
            return Ok(employees);
        }
    }
}
