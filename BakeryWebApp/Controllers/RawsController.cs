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
    public class RawsController : Controller
    {
        private readonly BakeryContext _context;

        public RawsController(BakeryContext context)
        {
            _context = context;
        }


        public IActionResult Index(string rawName, int page = 1)
        {
            if(rawName == null)
                HttpContext.Request.Cookies.TryGetValue("rawName", out rawName);
            else
                HttpContext.Response.Cookies.Append("rawName", rawName);
            IEnumerable<Raw> raws = _context.Raws;
            int pageSize = 15;
            if(rawName != null)
                raws = raws.Where(n => n.RawName.Contains(rawName));
            int count = raws.Count();
            raws = raws.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            RawViewModel viewModel = new RawViewModel
            {
                Raws = raws,
                PageViewModel = new PageViewModel(count, page, pageSize),
                RawName = rawName
            };
            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RawId,RawName")] Raw raw)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raw);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(raw);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raw = await _context.Raws.FindAsync(id);
            if (raw == null)
            {
                return NotFound();
            }
            return View(raw);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RawId,RawName")] Raw raw)
        {
            if (id != raw.RawId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raw);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RawExists(raw.RawId))
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
            return View(raw);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raw = await _context.Raws
                .FirstOrDefaultAsync(m => m.RawId == id);
            if (raw == null)
            {
                return NotFound();
            }

            return View(raw);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raw = await _context.Raws.FindAsync(id);
            var norms = _context.Norms.Where(n => n.RowId == id);
            _context.Norms.RemoveRange(norms);
            _context.Raws.Remove(raw);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RawExists(int id)
        {
            return _context.Raws.Any(e => e.RawId == id);
        }

        public IActionResult ClearCache()
        {
            HttpContext.Response.Cookies.Delete("rawName");
            return RedirectToAction("Index");
        }
    }
}
