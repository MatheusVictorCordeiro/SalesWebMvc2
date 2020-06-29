using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc2.Models;
using SalesWebMvc2.Models.ViewModels;
using SalesWebMvc2.Services;

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

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        //vai para a pagina Create.cshtml metodo get
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        public IActionResult Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();

            }
            return View(obj);
        }


        //para dizer que é o metodo post
        [HttpPost]
        //para prevenir ataques csrf - aproveita a sessaão de autenticação para enviar dados maliliosos
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)

        {

            // chamamos o service para salvar no banco de dados, e depois retornamos para a view index para mostrar a lista de vendendores.
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            _sellerService.Remove(id);
           return RedirectToAction(nameof(Index));

        }

        //usa a mesma logica do delete metodo get, vai buscar pra ver se o vendedor existe
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }



    }
}