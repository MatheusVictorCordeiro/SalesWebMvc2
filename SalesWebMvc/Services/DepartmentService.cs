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


        public List<Department> FindAll()
        {
            //o Order by é usado para ordenar a lista
            return _context.Department.OrderBy(p => p.Name).ToList();

        }
    }
}
