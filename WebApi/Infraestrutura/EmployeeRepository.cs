﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model;

namespace WebApi.Infraestrutura
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ConnectionContext _context =  new ConnectionContext();


        //Metodo Para cadastrar um usuario com Foto e armazenar a imagem na pasta Storage
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }


        //Retorna todos os usuarios do sistema
        public List<Employee> Get()
        {
            return _context.Employees.ToList();
        }

        //Retorna um usuario em especifico pelo ID informado
        public Employee Get(int id)
        {
            return _context.Employees.Find(id);
        }


        //Deleta um usuario pelo ID informado
        public Employee Delete(int id)
        {
            // Busca a entidade pelo ID
            var employee = _context.Employees.Find(id);

            // Verifica se o Employee foi encontrado
            if (employee != null)
            {
                // Obtenha o caminho da foto
                var photoPath = employee.photo;
                _context.Employees.Remove(employee); // Remove a entidade

                if (!string.IsNullOrEmpty(photoPath) && System.IO.File.Exists(photoPath))
                {
                    // Exclui a foto do sistema de arquivos
                    System.IO.File.Delete(photoPath);
                }
                _context.SaveChanges(); // Salva as mudanças no banco de dados
            }

            return employee; // Retorna o Employee removido, ou null se não foi encontrado
        }


        //Metodo para atualizar apenas dois campos do usuario / Nome e idade
        public Employee UpdateAgeName(int id, string name, int age)
        {
            // Busca o Employee existente pelo ID
            var employee = _context.Employees.Find(id);


            // Verifica se o Employee foi encontrado
            if (employee != null)
            {
                // Informa ao EF que as propriedades foram modificadas
                employee.UpdateName(name);
                employee.UpdateAge(age);

                // Informa ao EF que as propriedades foram modificadas
                _context.Entry(employee).Property(employee => employee.name).IsModified = true;
                _context.Entry(employee).Property(employee => employee.age).IsModified = true;

                // Salva as mudanças no banco de dados
                _context.SaveChanges();


            }

            return employee;
            
        }

    
    }
}
