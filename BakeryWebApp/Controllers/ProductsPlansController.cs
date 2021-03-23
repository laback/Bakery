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
    public class ProductsPlansController : Controller
    {
        private readonly BakeryContext _context;
        private IMemoryCache _memoryCache;

        public ProductsPlansController(BakeryContext context, IMemoryCache cache)
        {
            _context = context;
            _memoryCache = cache;
        }

        public IActionResult Index(DateTime planDate, string productName, SortState sortState, int countOfProd, int page = 1)
        {
            IEnumerable<ProductsPlan> productsPlans = _context.ProductsPlans.Include(p => p.DayPlan).Include(p => p.Product);
            if (planDate == default)
                _memoryCache.TryGetValue("productPlanDate", out planDate);
            else
            {
                _memoryCache.Set("productPlanDate", planDate);
                productsPlans = productsPlans.Where(p => p.DayPlan.Date.Equals(planDate));
            }
            if (productName == null)
                _memoryCache.TryGetValue("productNamePlan", out productName);
            else
            {
                _memoryCache.Set("productNamePlan", productName);
                productsPlans = productsPlans.Where(n => n.Product.ProductName.Contains(productName));
            }
            if (countOfProd == 0)
                _memoryCache.TryGetValue("countProductPlan", out countOfProd);
            else
            {
                _memoryCache.Set("countProductPlan", countOfProd);
                productsPlans = productsPlans.Where(p => p.Count == countOfProd);
            }
            if (sortState == SortState.No)
                _memoryCache.TryGetValue("productPlanSort", out sortState);
            else
            {
                _memoryCache.Set("productPlanSort", sortState);
                productsPlans = Sort(productsPlans, sortState);
            }
            int count = productsPlans.Count();
            int pageSize = 15;
            productsPlans = productsPlans.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ProductsPlansViewModel viewModel = new ProductsPlansViewModel
            {
                ProductsPlans = productsPlans,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                PlanDate = planDate,
                ProductName = productName,
                Count = countOfProd

            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["DayPlanId"] = new SelectList(_context.DayPlans, "DayPlanId", "Date");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsPlansViewModel productsPlansViewModel)
        {
            DayPlan dayPlans = _context.DayPlans.Where(d => d.Date.Equals(productsPlansViewModel.Date)).FirstOrDefault();
            if (dayPlans == null)
            {
                _context.DayPlans.Add(new DayPlan { Date = productsPlansViewModel.Date });
                await _context.SaveChangesAsync();
                dayPlans = _context.DayPlans.Where(d => d.Date.Equals(productsPlansViewModel.Date)).FirstOrDefault();
            }
            ProductsPlan productsPlan = productsPlansViewModel.ProductsPlan;
            productsPlan.DayPlanId = dayPlans.DayPlanId;
            if (ModelState.IsValid)
            {
                _context.Add(productsPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayPlanId"] = new SelectList(_context.DayPlans, "DayPlanId", "Date");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productsPlan);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsPlan = await _context.ProductsPlans.FindAsync(id);
            ProductsPlansViewModel productsPlansViewModel = new ProductsPlansViewModel
            {
                ProductsPlan = productsPlan
            };
            if (productsPlan == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productsPlansViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductsPlansViewModel productsPlansViewModel)
        {
            if (id != productsPlansViewModel.ProductsPlan.ProductPlan)
            {
                return NotFound();
            }
            DayPlan dayPlans = _context.DayPlans.Where(d => d.Date.Equals(productsPlansViewModel.Date)).FirstOrDefault();
            if (dayPlans == null)
            {
                _context.DayPlans.Add(new DayPlan { Date = productsPlansViewModel.Date });
                await _context.SaveChangesAsync();
                dayPlans = _context.DayPlans.Where(d => d.Date.Equals(productsPlansViewModel.Date)).FirstOrDefault();
            }
            ProductsPlan productsPlan = productsPlansViewModel.ProductsPlan;
            productsPlan.DayPlanId = dayPlans.DayPlanId;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsPlanExists(productsPlan.ProductPlan))
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
            ViewData["DayPlanId"] = new SelectList(_context.DayPlans, "DayPlanId", "Date");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(productsPlan);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsPlan = await _context.ProductsPlans
                .Include(p => p.DayPlan)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductPlan == id);
            if (productsPlan == null)
            {
                return NotFound();
            }

            return View(productsPlan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productsPlan = await _context.ProductsPlans.FindAsync(id);
            _context.ProductsPlans.Remove(productsPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsPlanExists(int id)
        {
            return _context.ProductsPlans.Any(e => e.ProductPlan == id);
        }
        private IEnumerable<ProductsPlan> Sort(IEnumerable<ProductsPlan> productsPlans, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.ProductPlanDateAcs:
                    productsPlans = productsPlans.OrderBy(d => d.DayPlan.Date);
                    break;
                case SortState.ProductPlanDateDecs:
                    productsPlans = productsPlans.OrderByDescending(d => d.DayPlan.Date);
                    break;
                case SortState.ProductPlanProductAcs:
                    productsPlans = productsPlans.OrderBy(p => p.Product.ProductName);
                    break;
                case SortState.ProductPlanProductDecs:
                    productsPlans = productsPlans.OrderByDescending(p => p.Product.ProductName);
                    break;
                case SortState.ProductPlanCountAcs:
                    productsPlans = productsPlans.OrderBy(c => c.Count);
                    break;
                case SortState.ProductPlanCountDecs:
                    productsPlans = productsPlans.OrderByDescending(c => c.Count);
                    break;
            }
            return productsPlans;
        }

        public IActionResult ClearFilter()
        {
            _memoryCache.Remove("productPlanDate");
            _memoryCache.Remove("productNamePlan");
            _memoryCache.Remove("countProductPlan");
            _memoryCache.Remove("productPlanSort");
            return RedirectToAction("Index");
        }
    }
}
