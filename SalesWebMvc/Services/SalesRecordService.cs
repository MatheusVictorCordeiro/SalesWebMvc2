using Microsoft.EntityFrameworkCore;
using SalesWebMvc2.Data;
using SalesWebMvc2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc2.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvc2Context _context;

        public SalesRecordService(SalesWebMvc2Context context)
        {
            _context = context;
        }


        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? dataMinima, DateTime? dataMaxima)
        {
            //essa consulta vai pegar o SalesRecord que é do tipo Dbset, e contruir um obj result do tipo IQueriable, e encima desse obj pode 
            //incrementar os detalhes da consulta.
            var result = from obj in _context.SalesRecord select obj;

            if (dataMinima.HasValue)
            {
                result = result.Where(x => x.Date >= dataMinima.Value);
            }
            if (dataMaxima.HasValue)
            {
                result = result.Where(x => x.Date <= dataMaxima.Value);
            }

            return await result
                //isso faz o Join das tabelas
                .Include(x => x.Seller) //tabela de vendedor
                .Include(x => x.Seller.Department)//inclui o departamento
                .OrderByDescending(x=> x.Date)//Ordena por data
                .ToListAsync();

        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? dataMinima, DateTime? dataMaxima)
        {
            //essa consulta vai pegar o SalesRecord que é do tipo Dbset, e contruir um obj result do tipo IQueriable, e encima desse obj pode 
            //incrementar os detalhes da consulta.
            var result = from obj in _context.SalesRecord select obj;

            if (dataMinima.HasValue)
            {
                result = result.Where(x => x.Date >= dataMinima.Value);
            }
            if (dataMaxima.HasValue)
            {
                result = result.Where(x => x.Date <= dataMaxima.Value);
            }

            return await result
                //isso faz o Join das tabelas
                .Include(x => x.Seller) //tabela de vendedor
                .Include(x => x.Seller.Department)//inclui o departamento
                .OrderByDescending(x => x.Date)//Ordena por data
                // para agrupar, o retorno não pode ser majs uma lista normal e sim uma lista de Igrouping
                .GroupBy(x=> x.Seller.Department)
                .ToListAsync();

        }





    }
}
