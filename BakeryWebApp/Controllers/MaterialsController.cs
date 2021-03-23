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
    public class MaterialsController : Controller
    {
        private readonly BakeryContext _context;

        public MaterialsController(BakeryContext context)
        {
            _context = context;
        }

        public IActionResult Index(string materialName, int page = 1)
        {
            if (materialName == null)
                HttpContext.Request.Cookies.TryGetValue("materialName", out materialName);
            else
                HttpContext.Response.Cookies.Append("materialName", materialName);
            IEnumerable<Material> materials = _context.Materials;
            int pageSize = 15;
            if (materialName != null)
                materials = materials.Where(n => n.MaterialName.Contains(materialName));
            int count = materials.Count();
            materials = materials.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            MaterialViewModel viewModel = new MaterialViewModel
            {
                Materials = materials,
                PageViewModel = new PageViewModel(count, page, pageSize),
                MaterialName = materialName

            };
            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaterialId,MaterialName")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaterialId,MaterialName")] Material material)
        {
            if (id != material.MaterialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.MaterialId))
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
            return View(material);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.MaterialId == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            var materialProduct = _context.ProductsMaterials.Where(m => m.MaterialId == id);
            _context.ProductsMaterials.RemoveRange(materialProduct);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.MaterialId == id);
        }

        public IActionResult ClearCache()
        {
            HttpContext.Response.Cookies.Delete("materialName");
            return RedirectToAction("Index");
        }
    }
}
