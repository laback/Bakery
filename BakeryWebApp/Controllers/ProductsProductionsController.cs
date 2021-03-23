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
    public class ProductsProductionsController : Controller
    {
        private readonly BakeryContext _context;
        private IMemoryCache _memoryCache;

        public ProductsProductionsController(BakeryContext context, IMemoryCache cache)
        {
            _context = context;
            _memoryCache = cache;
        }

        public IActionResult Index(DateTime productionDate, string productName, SortState sortState, int countOfProd, int page = 1)
        {
            IEnumerable<ProductsProduction> productsProductions = _context.ProductsProductions.Include(p => p.DayProduction).Include(p => p.Product);
            if (productionDate == default)
                _memoryCache.TryGetValue("productProductionDate", out productionDate);
            else
            {
                _memoryCache.Set("productProductionDate", productionDate);
                productsProductions = productsProductions.Where(p => p.DayProduction.Date.Equals(productionDate));
            }
            if (productName == null)
                _memoryCache.TryGetValue("productProductionName", out productName);
            else
            {
                _memoryCache.Set("productProductionName", productName);
                productsProductions = productsProductions.Where(n => n.Product.ProductName.Contains(productName));
            }
            if (countOfProd == 0)
                _memoryCache.TryGetValue("countProductProduction", out countOfProd);
            else
            {
                _memoryCache.Set("countProductProduction", countOfProd);
                productsProductions = productsProductions.Where(p => p.Count == countOfProd);
            }
            if (sortState == SortState.No)
                _memoryCache.TryGetValue("productProductionSort", out sortState);
            else
            {
                _memoryCache.Set("productProductionSort", sortState);
                productsProductions = Sort(productsProductions, sortState);
            }
            int count = productsProductions.Count();
            int pageSize = 15;
            productsProductions = productsProductions.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ProductProdactionViewModel viewModel = new ProductProdactionViewModel
            {
                ProductsProductions = productsProductions,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                ProductionDate = productionDate,
                ProductName = productName,
                Count = countOfProd

            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["DayProductionId"] = new SelectList(_context.DayProductions, "DayProductionId", "Date");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductProdactionViewModel productProdactionViewModel)
        {
            DayProduction dayProduction = _context.DayProductions.Where(d => d.Date.Equals(productProdactionViewModel.Date)).FirstOrDefault();
            if (dayProduction == null)
            {
                _context.DayProductions.Add(new DayProduction { Date = productProdactionViewModel.Date });
                await _context.SaveChangesAsync();
                dayProduction = _context.DayProductions.Where(d => d.Date.Equals(productProdactionViewModel.Date)).FirstOrDefault();
            }
            ProductsProduction productsProduction = productProdactionViewModel.ProductsProduction;
            productsProduction.DayProductionId = dayProduction.DayProductionId;
            if (ModelState.IsValid)
            {
                _context.Add(productsProduction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayProductionId"] = new SelectList(_context.DayProductions, "DayProductionId", "Date");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productsProduction);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productProduction = await _context.ProductsProductions.FindAsync(id);
            ProductProdactionViewModel productProdactionViewModel = new ProductProdactionViewModel
            {
                ProductsProduction = productProduction
            };
            var productsProduction = await _context.ProductsProductions.FindAsync(id);
            if (productsProduction == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productProdactionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductProdactionViewModel productProdactionViewModel)
        {
            if (id != productProdactionViewModel.ProductsProduction.ProductProductionId)
            {
                return NotFound();
            }
            DayProduction dayProduction = _context.DayProductions.Where(d => d.Date.Equals(productProdactionViewModel.Date)).FirstOrDefault();
            if (dayProduction == null)
            {
                _context.DayProductions.Add(new DayProduction { Date = productProdactionViewModel.Date });
                await _context.SaveChangesAsync();
                dayProduction = _context.DayProductions.Where(d => d.Date.Equals(productProdactionViewModel.Date)).FirstOrDefault();
            }
            ProductsProduction productsProduction = productProdactionViewModel.ProductsProduction;
            productsProduction.DayProductionId = dayProduction.DayProductionId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsProduction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsProductionExists(productsProduction.ProductProductionId))
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
            ViewData["DayProductionId"] = new SelectList(_context.DayProductions, "DayProductionId", "Date");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productsProduction);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsProduction = await _context.ProductsProductions
                .Include(p => p.DayProduction)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductProductionId == id);
            if (productsProduction == null)
            {
                return NotFound();
            }

            return View(productsProduction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productsProduction = await _context.ProductsProductions.FindAsync(id);
            _context.ProductsProductions.Remove(productsProduction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<ProductsProduction> Sort(IEnumerable<ProductsProduction> productsProductions, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.ProductProductionDateAcs:
                    productsProductions = productsProductions.OrderBy(d => d.DayProduction.Date);
                    break;
                case SortState.ProductProductionDateDecs:
                    productsProductions = productsProductions.OrderByDescending(d => d.DayProduction.Date);
                    break;
                case SortState.ProductProductionNameAcs:
                    productsProductions = productsProductions.OrderBy(p => p.Product.ProductName);
                    break;
                case SortState.ProductProductionNameDecs:
                    productsProductions = productsProductions.OrderByDescending(p => p.Product.ProductName);
                    break;
                case SortState.ProductProductionCountAcs:
                    productsProductions = productsProductions.OrderBy(c => c.Count);
                    break;
                case SortState.ProductProductionCountDecs:
                    productsProductions = productsProductions.OrderByDescending(c => c.Count);
                    break;
            }
            return productsProductions;
        }

        private bool ProductsProductionExists(int id)
        {
            return _context.ProductsProductions.Any(e => e.ProductProductionId == id);
        }
        public IActionResult ClearFilter()
        {
            _memoryCache.Remove("productProductionDate");
            _memoryCache.Remove("productProductionName");
            _memoryCache.Remove("countProductProduction");
            _memoryCache.Remove("productProductionSort");
            return RedirectToAction("Index");
        }
    }
}
