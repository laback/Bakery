using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeryWebApp;
using BakeryWebApp.ViewModels;

namespace BakeryWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BakeryContext _context;

        public ProductsController(BakeryContext context)
        {
            _context = context;
        }

        public IActionResult Index(string productName, int page = 1)
        {
            if (productName == null)
                HttpContext.Request.Cookies.TryGetValue("productName", out productName);
            else
                HttpContext.Response.Cookies.Append("productName", productName);
            IEnumerable<Product> products = _context.Products;
            int pageSize = 15;
            if (productName != null)
                products = products.Where(n => n.ProductName.Contains(productName));
            int count = products.Count();
            products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ProductViewModel viewModel = new ProductViewModel
            {
                Products = products,
                PageViewModel = new PageViewModel(count, page, pageSize),
                ProductName = productName
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productMaterial = _context.ProductsMaterials.Where(p => p.ProductId == id);
            _context.ProductsMaterials.RemoveRange(productMaterial);
            var productRaw = _context.Norms.Where(p => p.ProductId == id);
            _context.Norms.RemoveRange(productRaw);
            var productPlan = _context.ProductsPlans.Where(p => p.ProductId == id);
            _context.ProductsPlans.RemoveRange(productPlan);
            var productProduction = _context.ProductsProductions.Where(p => p.ProductId == id);
            _context.ProductsProductions.RemoveRange(productProduction);
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public IActionResult ClearCache()
        {
            HttpContext.Response.Cookies.Delete("productName");
            return RedirectToAction("Index");
        }
    }
}
