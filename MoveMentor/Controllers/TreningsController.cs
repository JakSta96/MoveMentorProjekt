using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoveMentor.Data;
using MoveMentor.Models;

namespace MoveMentor.Controllers
{
    [Authorize]
    public class TreningsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TreningsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Trenings
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var context = _context.Trening
                .Include(t => t.SportType)
                .Include(t => t.User)
                .Where(t => t.UserId == userId);

            return View(await context.ToListAsync());
        }

        // GET: Trenings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trening == null)
            {
                return NotFound();
            }

            var trening = await _context.Trening
                .Include(t => t.SportType)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trening == null)
            {
                return NotFound();
            }

            return View(trening);
        }

        // GET: Trenings/Create
        public IActionResult Create()
        {
            var sportTypes = _context.SportType.ToList();
            ViewBag.SportTypeId = new SelectList(_context.SportType, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Trenings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateTimeStart,DateTimeEnd,SportTypeId,UserId,Comment")] Trening trening)
        {
            trening.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _context.Add(trening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SportTypeId"] = new SelectList(_context.SportType, "Id", "Id", trening.SportTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", trening.UserId);
            return View(trening);
        }

        // GET: Trenings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trening == null)
            {
                return NotFound();
            }

            var trening = await _context.Trening.FindAsync(id);
            if (trening == null)
            {
                return NotFound();
            }
            var sportTypes = _context.SportType.ToList();
            ViewBag.SportTypeId = new SelectList(sportTypes, "Id", "Name", trening.SportTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", trening.UserId);
            return View(trening);
        }

        // POST: Trenings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTimeStart,DateTimeEnd,SportTypeId,UserId,Comment")] Trening trening)
        {
            if (id != trening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreningExists(trening.Id))
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
            ViewData["SportTypeId"] = new SelectList(_context.SportType, "Id", "Id", trening.SportTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", trening.UserId);
            return View(trening);
        }

        // GET: Trenings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trening == null)
            {
                return NotFound();
            }

            var trening = await _context.Trening
                .Include(t => t.SportType)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trening == null)
            {
                return NotFound();
            }

            return View(trening);
        }

        // POST: Trenings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trening == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Trening'  is null.");
            }
            var trening = await _context.Trening.FindAsync(id);
            if (trening != null)
            {
                _context.Trening.Remove(trening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreningExists(int id)
        {
            return (_context.Trening?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
