﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Domain.Model
{
    [Table("employee")]

    public class Employee
    {
        [Key]
        public int id { get; private set; }
        public string name { get; private set; }
        public int age { get; private set; }
        public string? photo { get; private set; }

        public Employee(string name, int age, string photo)
        {

            this.name = name;
            this.age = age;
            this.photo = photo;
        }



        // Construtor personalizado que aceita apenas name e age
        public Employee(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public Employee()
        {
        }

        // Métodos de atualização para update parcial(HTTP -> PUTCH)
        public void UpdateName(string name)
        {
            this.name = name;
        }

        public void UpdateAge(int age)
        {
            this.age = age;
        }


    }
}
