using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Marathon.Data;
using marathon.Models;
using Marathon.Views;

namespace Marathon.Controllers
{
    public class MarathonentitiesController : Controller
    {
        private readonly MarathonContext _context;

        public MarathonentitiesController(MarathonContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> GetImageAsync(int MaratnonId)
        {
            var marathonentity = await _context.Marathonentity
    .FirstOrDefaultAsync(m => m.MarathonId == MaratnonId);

            if (marathonentity==null) return null;
            if (marathonentity.Image == null) return null;
            return new FileContentResult(marathonentity.Image,"image/jpeg");
        }

        // GET: Marathonentities
        public async Task<IActionResult> Index()
        {
            var marathonEntities = await _context.Marathonentity.Include(m => m.Manager).ToListAsync();

            return View(marathonEntities);
        }


        // GET: Marathonentities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marathonentity = await _context.Marathonentity
                .Include(m => m.Manager)
                .FirstOrDefaultAsync(m => m.MarathonId == id);
            if (marathonentity == null)
            {
                return NotFound();
            }

            return View(marathonentity);
        }

        // GET: Marathonentities/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Manager, "ManagerId", "ManagerName");
            return View();
        }

        // POST: Marathonentities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarathonId,MarathonName,MarathonData,MarathonPlace,MarathonDetails,MarathonSponsor,ManagerId")] Marathonentity marathonentity, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    byte[] avatarData = new byte[image.Length];
                    await image.CopyToAsync(new MemoryStream(avatarData));
                    marathonentity.Image = avatarData;
                }
                _context.Add(marathonentity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Manager, "ManagerId", "ManagerName", marathonentity.ManagerId);
            return View(marathonentity);
        }

        // GET: Marathonentities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marathonentity = await _context.Marathonentity.FindAsync(id);
            if (marathonentity == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Manager, "ManagerId", "ManagerName", marathonentity.ManagerId);
            return View(marathonentity);
        }

        // POST: Marathonentities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarathonId,MarathonName,MarathonData,MarathonPlace,MarathonDetails,MarathonSponsor,ManagerId")] Marathonentity marathonentity, IFormFile image)
        {
            if (id != marathonentity.MarathonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    byte[] avatarData = new byte[image.Length];
                    await image.CopyToAsync(new MemoryStream(avatarData));
                    marathonentity.Image = avatarData;
                }

                try
                {
                    _context.Update(marathonentity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarathonentityExists(marathonentity.MarathonId))
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
            ViewData["ManagerId"] = new SelectList(_context.Manager, "ManagerId", "ManagerName", marathonentity.ManagerId);
            return View(marathonentity);
        }

        // GET: Marathonentities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marathonentity = await _context.Marathonentity
                .Include(m => m.Manager)
                .FirstOrDefaultAsync(m => m.MarathonId == id);
            if (marathonentity == null)
            {
                return NotFound();
            }

            return View(marathonentity);
        }

        public bool sighup(int id)
        {
            return _context.Check.Any(e => e.MarathonId == id);
        }

        // POST: Marathonentities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (sighup(id)) return Content("<script>alert('该赛事有报名人数，不能删除！');parent.location.href='/Marathonentities/Index'</script>", "text/html", System.Text.Encoding.GetEncoding("GB2312"));
            var marathonentity = await _context.Marathonentity.FindAsync(id);

            if (marathonentity != null)
            {
                _context.Marathonentity.Remove(marathonentity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarathonentityExists(int id)
        {
            return _context.Marathonentity.Any(e => e.MarathonId == id);
        }
    }
}
