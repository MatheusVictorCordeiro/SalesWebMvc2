using Microsoft.EntityFrameworkCore;
using SalesWebMvc2.Data;
using SalesWebMvc2.Models;
using SalesWebMvc2.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
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

        //transformar para assincrono, ou seja, para a aplicação não ficar bloqueada enquanto é feita a consulta ao banco de dados.
        // temos que renomear o FindAll para FindAllAsync, é um padrão adotado na linguagem.Task é um objeto que encapsula o processamento assincromo
        //o ToList é sincrono, temos que usar o ToListAsync para chamadas assincronas

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            //o incluide irá dar join na tabela de department.isso é eagerLoading, ele carrega outros objetos associados
            return await _context.Seller.Include(obj=> obj.Department).FirstOrDefaultAsync(p => p.Id == id);
        }

        //para transformar essa em assincrona, tiramos o void e adcionamos o Task
        //a savechanges que realmente acessa o banco, então ela que deve ter o async no final

        public async Task InsertAsync(Seller obj)
           
        {
            

            _context.Add(obj);
          await _context.SaveChangesAsync();

        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller seller)
        {
            //se não exixtir no banco de dados um vendedor com o mesmo Id.

            bool hasAny = await _context.Seller.AnyAsync(p => p.Id == seller.Id);
            if (!hasAny)
            {

                throw new NotFoundException("Não foi encontrado nenhum vendedor com esse Id");
            }
            try
            {
                _context.Update(seller);
                //o Update pode gerar um conflito de concorrencia de  dados, gerando uma exceção DbConcurrency Exception
               await _context.SaveChangesAsync();


                // aqui eu estou interceptando uma excessão do nivel de acesso a dados e estou relançando essa exceção em nivel de serviço
                // para respeitar a camada mvc, controladora conversa com o serviço
            }catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
            
        }

    }
}
