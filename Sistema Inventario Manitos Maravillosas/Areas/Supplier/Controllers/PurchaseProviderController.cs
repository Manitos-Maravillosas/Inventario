﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Models;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    [Authorize(Roles = "Administrador")]
    public class PurchaseProviderController : Controller
    {
        private readonly IPurchaseProviderService _PurchaseProviderService;
        private readonly IProductService _ProductService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IBusinessService _businessService;
        private readonly IProviderService _ProviderService;
        public PurchaseProviderController(IPurchaseProviderService purchaseProviderService, IProductService productService, IProviderService providerService, IBusinessService businessService)
        {
            _PurchaseProviderService = purchaseProviderService;
            _ProductService = productService;
            _ProviderService = providerService;
            _businessService = businessService;
        }
        // GET: PurchaseProviderController
        public ActionResult Index()
        {
            var purchases = _PurchaseProviderService.GetAll();

            return View(purchases);
        }

        // GET: PurchaseProviderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PurchaseProviderController/Create
        public ActionResult Create()
        {
            PurchaseProvider purchaseProvider = new PurchaseProvider();
            purchaseProvider.Products = _ProductService.GetAll();

            var businessNames = _businessService.GetBusinessNames();
            ViewBag.BusinessNames = new SelectList(businessNames);

            var providerNames = _ProviderService.GetProviderNames();
            ViewBag.ProviderNames = new SelectList(providerNames);

            return View(purchaseProvider);
        }

        // POST: PurchaseProviderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseProvider purchaseProvider)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _PurchaseProviderService.Add(purchaseProvider);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Compra a Inventario agregada correctamente!";

            }
            purchaseProvider.Products = _ProductService.GetAll();
            ViewBag.BusinessNames = new SelectList(_businessService.GetBusinessNames());
            ViewBag.ProviderNames = new SelectList(_ProviderService.GetProviderNames());
            return View(purchaseProvider);
        }

        // GET: PurchaseProviderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PurchaseProviderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PurchaseProviderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PurchaseProviderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
