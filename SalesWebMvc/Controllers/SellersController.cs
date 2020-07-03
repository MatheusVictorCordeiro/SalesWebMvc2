using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SalesWebMvc2.Models;
using SalesWebMvc2.Models.ViewModels;
using SalesWebMvc2.Services;
using SalesWebMvc2.Services.Exceptions;

namespace SalesWebMvc2.Controllers
{
    public class SellersController : Controller
    {
        //dependencia para o service, para poder usar os services
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        //depois de deixar os services Asyncronos, precisamos ajustar a controladora, ao inves de IActionResult, vai ser Task<IActionResult>
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        //vai para a pagina Create.cshtml metodo get
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                //vamos chamar a função Erro(), pra passar a mensagem que a funçao Error espera , criamos um objeto anonimo
                return RedirectToAction(nameof(Error), new { message = "Id não foi fornecido" });
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                //vamos chamar a função Erro(), pra passar a mensagem que a funçao Error espera , criamos um objeto anonimo
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });

            }
            return View(obj);
        }


        //para dizer que é o metodo post
        [HttpPost]
        //para prevenir ataques csrf - aproveita a sessaão de autenticação para enviar dados maliliosos
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)

        {
            //essa validação na controladora é para caso o usuario desative o js, fazendo com que aquela validação na parte do cliente nao funcione
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Departments = departments, Seller = seller };
                return View(viewModel);
            }
            // chamamos o service para salvar no banco de dados, e depois retornamos para a view index para mostrar a lista de vendendores.
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try { 
            await _sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
                }catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        //usa a mesma logica do delete metodo get, vai buscar pra ver se o vendedor existe
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                //vamos chamar a função Erro(), pra passar a mensagem que a funçao Error espera , criamos um objeto anonimo
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido" }); ;
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                //vamos chamar a função Erro(), pra passar a mensagem que a funçao Error espera , criamos um objeto anonimo
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }
            return View(obj);
        }
        // o Id esta opcional para evitar erros, a verdade ele é obrigatorio
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //vamos chamar a função Erro(), pra passar a mensagem que a funçao Error espera , criamos um objeto anonimo
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido" }); ;
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                //vamos chamar a função Erro(), pra passar a mensagem que a funçao Error espera , criamos um objeto anonimo
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Departments = departments, Seller = obj };
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Seller seller)
        {

            //essa validação na controladora é para caso o usuario desative o js, fazendo com que aquela validação na parte do cliente nao funcione
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Departments = departments, Seller = seller };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                
                return RedirectToAction(nameof(Error), new { message = "Id não corresponde" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
              return  RedirectToAction(nameof(Index));

                //vamos usar o super tipo da exceção, dae ela serve para a DbConcurrencyException e NotFoundException
            }catch(ApplicationException e)
            {
                //nesse caso vamos usar a mensagem da exceção.
                return RedirectToAction(nameof(Error), new { message = e.Message });

            }
            

        }

        //essa ação é para ir para a pagina de erro
        public IActionResult Error(String message)
        {

            var viewModel = new ErrorViewModel
            {
                Message = message,
                //macete do framework pra pegar o id interno da requisição
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier

            };

            return View(viewModel);
        }

    }
}