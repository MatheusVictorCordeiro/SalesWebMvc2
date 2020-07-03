using Microsoft.EntityFrameworkCore;
using SalesWebMvc2.Data;
using SalesWebMvc2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc2.Services
{
    public class DepartmentService
    {

        private readonly SalesWebMvc2Context _context;

        public DepartmentService(SalesWebMvc2Context context)
        {
            _context = context;
        }

        //transformar para assincrono, ou seja, para a aplicação não ficar bloqueada enquanto é feita a consulta ao banco de dados.
        // temos que renomear o FindAll para FindAllAsync, é um padrão adotado na linguagem.Task é um objeto que encapsula o processamento assincromo
        //o ToList é sincrono, temos que usar o ToListAsync para chamadas assincronas
        public async Task<List<Department>> FindAllAsync()
        {
            //o Order by é usado para ordenar a lista
            return await _context.Department.OrderBy(p => p.Name).ToListAsync();

        }
    }
}
