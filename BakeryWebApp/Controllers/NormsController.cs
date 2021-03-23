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
    public class NormsController : Controller
    {
        private readonly BakeryContext _context;
        private IMemoryCache _memoryCache;

        public NormsController(BakeryContext context, IMemoryCache cache)
        {
            _context = context;
            _memoryCache = cache;
        }

        public IActionResult Index(string rawName, string productName, double quantity, SortState sortState, int page = 1)
        {
            IEnumerable<Norm> norms = _context.Norms.Include(r => r.Raw).Include(p => p.Product);
            if (rawName == null)
                _memoryCache.TryGetValue("rawNameNorm", out rawName);
            else
            {
                _memoryCache.Set("rawNameNorm", rawName);
                norms = norms.Where(n => n.Raw.RawName.Contains(rawName));
            }
            if (productName == null)
                _memoryCache.TryGetValue("productNameNorm", out productName);
            else
            {
                _memoryCache.Set("productNameNorm", productName);
                norms = norms.Where(n => n.Product.ProductName.Contains(productName));
            }
            if (quantity == 0)
                _memoryCache.TryGetValue("quanityNorm", out quantity);
            else
            {
                _memoryCache.Set("quantityNorm", quantity);
                norms = norms.Where(n => n.Quantity == quantity);
            }
            if (sortState == SortState.No)
                _memoryCache.TryGetValue("normSort", out sortState);
            else
            {
                _memoryCache.Set("normSort", sortState);
                norms = Sort(norms, sortState);
            }
            int count = norms.Count();
            int pageSize = 15;
            norms = norms.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            NormViewModel viewModel = new NormViewModel
            {
                Norms = norms,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                RawName = rawName,
                ProductionName = productName,
                Quantity = quantity

            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["RowId"] = new SelectList(_context.Raws, "RawId", "RawName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NormId,RowId,ProductId,Quantity")] Norm norm)
        {

            if (norm.Quantity > 0)
            {
                _context.Add(norm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["RowId"] = new SelectList(_context.Raws, "RawId", "RawName");
            return View(norm);
        }

        // GET: Norms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var norm = await _context.Norms.FindAsync(id);
            if (norm == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["RowId"] = new SelectList(_context.Raws, "RawId", "RawName");
            return View(norm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NormId,RowId,ProductId,Quantity")] Norm norm)
        {
            if (id != norm.NormId)
            {
                return NotFound();
            }

            if (norm.Quantity > 0)
            {
                try
                {
                    _context.Update(norm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NormExists(norm.NormId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["RowId"] = new SelectList(_context.Raws, "RawId", "RawName");
            return View(norm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var norm = await _context.Norms
                .Include(n => n.Product)
                .Include(n => n.Raw)
                .FirstOrDefaultAsync(m => m.NormId == id);
            if (norm == null)
            {
                return NotFound();
            }

            return View(norm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var norm = await _context.Norms.FindAsync(id);
            _context.Norms.Remove(norm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NormExists(int id)
        {
            return _context.Norms.Any(e => e.NormId == id);
        }

        private IEnumerable<Norm> Sort(IEnumerable<Norm> norms, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.NormRawNameAcs:
                    norms = norms.OrderBy(r => r.Raw.RawName);
                    break;
                case SortState.NormRawNameDecs:
                    norms = norms.OrderByDescending(r => r.Raw.RawName);
                    break;
                case SortState.NormProductNameAcs:
                    norms = norms.OrderBy(p => p.Product.ProductName);
                    break;
                case SortState.NormProductNameDecs:
                    norms = norms.OrderByDescending(p => p.Product.ProductName);
                    break;
                case SortState.NormQuantityAcs:
                    norms = norms.OrderBy(q => q.Quantity);
                    break;
                case SortState.NormQuantityDecs:
                    norms = norms.OrderByDescending(q => q.Quantity);
                    break;
            }
            return norms;
        }

        public IActionResult ClearFilter()
        {
            _memoryCache.Remove("rawNameNorm");
            _memoryCache.Remove("productNameNorm");
            _memoryCache.Remove("quanityNorm");
            _memoryCache.Remove("normSort");
            return RedirectToAction("Index");
        }
    }
}
