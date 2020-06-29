using Microsoft.EntityFrameworkCore;
using SalesWebMvc2.Data;
using SalesWebMvc2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc2.Services
{
    public class SellerService
    {
        //readonly é para nao poder alterar a dependencia.
        private readonly SalesWebMvc2Context _context;

        public SellerService(SalesWebMvc2Context context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public Seller FindById(int id)
        {
            //o incluide irá dar join na tabela de department.isso é eagerLoading, ele carrega outros objetos associados
            return _context.Seller.Include(obj=> obj.Department).FirstOrDefault(p => p.Id == id);
        }

        public void Insert(Seller obj)
           
        {
            

            _context.Add(obj);
            _context.SaveChanges();

        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }



    }
}
