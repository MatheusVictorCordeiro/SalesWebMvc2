using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc2.Models;
using SalesWebMvc2.Services;

namespace SalesWebMvc2.Controllers
{
    public class SellersController : Controller
    {
        //dependencia para o service, para poder usar os services
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        //vai para a pagina Create.cshtml metodo get
        public IActionResult Create()
        {
            return View();
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



    }
}