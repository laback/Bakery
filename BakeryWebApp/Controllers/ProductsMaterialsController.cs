using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeryWebApp;
using Microsoft.Extensions.Caching.Memory;
using BakeryWebApp.ViewModels;

namespace BakeryWebApp.Controllers
{
    public class ProductsMaterialsController : Controller
    {
        private readonly BakeryContext _context;
        private IMemoryCache _memoryCache;

        public ProductsMaterialsController(BakeryContext context, IMemoryCache cache)
        {
            _context = context;
            _memoryCache = cache;
        }
        public IActionResult Index(string materialName, string productName, double quantity, SortState sortState, int page = 1)
        {
            IEnumerable<ProductsMaterial> productsMaterials = _context.ProductsMaterials.Include(m => m.Material).Include(p => p.Product);
            if (materialName == null)
                _memoryCache.TryGetValue("materialNameProductMaterial", out materialName);
            else
            {
                _memoryCache.Set("materialNameProductMaterial", materialName);
                productsMaterials = productsMaterials.Where(n => n.Material.MaterialName.Contains(materialName));
            }
            if (productName == null)
                _memoryCache.TryGetValue("productNameProductMaterial", out productName);
            else
            {
                _memoryCache.Set("productNameProductMaterial", productName);
                productsMaterials = productsMaterials.Where(n => n.Product.ProductName.Contains(productName));
            }
            if (quantity == 0)
                _memoryCache.TryGetValue("quanityProductMaterial", out quantity);
            else
            {
                _memoryCache.Set("quantityProductMaterial", quantity);
                productsMaterials = productsMaterials.Where(n => n.Quantity == quantity);
            }
            if (sortState == SortState.No)
                _memoryCache.TryGetValue("productMaterialSort", out sortState);
            else
            {
                _memoryCache.Set("productMaterialSort", sortState);
                productsMaterials = Sort(productsMaterials, sortState);
            }
            int count = productsMaterials.Count();
            int pageSize = 15;
            productsMaterials = productsMaterials.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ProductMaterialViewModel viewModel = new ProductMaterialViewModel
            {
                ProductsMaterials = productsMaterials,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                MaterialName = materialName,
                ProductionName = productName,
                Quantity = quantity

            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "MaterialName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductMaterial,MaterialId,ProductId,Quantity")] ProductsMaterial productsMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productsMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "MaterialName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productsMaterial);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsMaterial = await _context.ProductsMaterials.FindAsync(id);
            if (productsMaterial == null)
            {
                return NotFound();
            }
            ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "MaterialName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productsMaterial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductMaterial,MaterialId,ProductId,Quantity")] ProductsMaterial productsMaterial)
        {
            if (id != productsMaterial.ProductMaterial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsMaterialExists(productsMaterial.ProductMaterial))
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
            ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "MaterialName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productsMaterial);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsMaterial = await _context.ProductsMaterials
                .Include(p => p.Material)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductMaterial == id);
            if (productsMaterial == null)
            {
                return NotFound();
            }

            return View(productsMaterial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productsMaterial = await _context.ProductsMaterials.FindAsync(id);
            _context.ProductsMaterials.Remove(productsMaterial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsMaterialExists(int id)
        {
            return _context.ProductsMaterials.Any(e => e.ProductMaterial == id);
        }
        private IEnumerable<ProductsMaterial> Sort(IEnumerable<ProductsMaterial> productsMaterials, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.ProductMaterialMaterialAcs:
                    productsMaterials = productsMaterials.OrderBy(m => m.Material.MaterialName);
                    break;
                case SortState.ProductMaterialMaterialDecs:
                    productsMaterials = productsMaterials.OrderByDescending(m => m.Material.MaterialName);
                    break;
                case SortState.ProductMaterialProductAcs:
                    productsMaterials = productsMaterials.OrderBy(p => p.Product.ProductName);
                    break;
                case SortState.ProductMaterialProductDecs:
                    productsMaterials = productsMaterials.OrderBy(p => p.Product.ProductName);
                    break; ;
                case SortState.ProductMaterialQuantityAcs:
                    productsMaterials = productsMaterials.OrderBy(q => q.Quantity);
                    break;
                case SortState.ProductMaterialQuantityDecs:
                    productsMaterials = productsMaterials.OrderBy(q => q.Quantity);
                    break;
            }
            return productsMaterials;
        }

        public IActionResult ClearFilter()
        {
            _memoryCache.Remove("materialNameProductMaterial");
            _memoryCache.Remove("productNameProductMaterial");
            _memoryCache.Remove("quanityProductMaterial");
            _memoryCache.Remove("productMaterialSort");
            return RedirectToAction("Index");
        }
    }
}
