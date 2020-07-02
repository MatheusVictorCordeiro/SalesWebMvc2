using Microsoft.AspNetCore.Rewrite.Internal.IISUrlRewrite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace SalesWebMvc2.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //esse data type irá transformar o que antes era texto em um link de email no crud.
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //o Display é para não aparecer na tela de usuario escrito BirthDate, para personalizar um nome.
        [Display(Name="Data de nascimento")]
        //o Data type é para personalizar o tipo de entrada de dados, existem diversas opçoes, cartão de credito, moeda, data.
        // nesse caso o Datatype.Date foi para não aparecer a hora e os minutos
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        //o Display é para não aparecer na tela de usuario escrito BirthDate, para personalizar um nome.
        [Display(Name = "Salario base")]

        // para deixar com duas casas decimais, usa-se o DisplayFormat
        [DisplayFormat(DataFormatString ="{0:f2}")]
        public double BaseSalary { get; set; }

        public Department Department { get; set; }


        // o tipo int não pode ser nulo
        public int DepartmentId { get; set; }

        public ICollection<SalesRecord> Sales = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {

            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales( DateTime initial,DateTime final)
        {
            return Sales.Where(p => p.Date >= initial && p.Date <= final).Sum(p => p.Amount);
        }
        


        




    }

    
}
