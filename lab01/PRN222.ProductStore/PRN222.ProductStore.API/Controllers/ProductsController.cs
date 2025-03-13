using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN222.ProductStore.Service.DTOs;
using PRN222.ProductStore.Service.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PRN222.ProductStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ICategoryService categoryService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _productService.GetProductsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products.");
                TempData["Message"] = "Something went wrong.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return product != null ? View(product) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product details.");
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategoriesAsync(), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            if (!ModelState.IsValid) return View(productDto);

            try
            {
                await _productService.AddProductAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product.");
                TempData["Message"] = "Something went wrong.";
                return View(productDto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null) return NotFound();

                ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategoriesAsync(), "CategoryId", "CategoryName", product.CategoryId);
                return View(new UpdateProductDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    UnitsInStock = product.UnitsInStock,
                    UnitPrice = product.UnitPrice
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product for editing.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductDto productDto)
        {
            if (id != productDto.ProductId) return NotFound();
            if (!ModelState.IsValid) return View(productDto);

            try
            {
                await _productService.UpdateProductAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product.");
                TempData["Message"] = "Something went wrong.";
                return View(productDto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return product != null ? View(product) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product for deletion.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userRole = HttpContext.Session.GetString("UserRole");

                if (userRole != "2")
                {
                    TempData["Message"] = "You are not authorized to delete products.";

                    var product = await _productService.GetProductByIdAsync(id);
                    if (product == null) return NotFound();

                    return View("Delete", product); // Stay on the Delete page
                }

                await _productService.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Something went wrong.";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
