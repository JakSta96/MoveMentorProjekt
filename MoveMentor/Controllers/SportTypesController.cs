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
    public class SportTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SportTypesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SportTypes
        public async Task<IActionResult> Index()
        {
              return _context.SportType != null ? 
                          View(await _context.SportType.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SportType'  is null.");
        }

        // GET: SportTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SportType == null)
            {
                return NotFound();
            }

            var sportType = await _context.SportType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportType == null)
            {
                return NotFound();
            }

            return View(sportType);
        }

        // GET: SportTypes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SportTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name")] SportType sportType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sportType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sportType);
        }

        // GET: SportTypes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SportType == null)
            {
                return NotFound();
            }

            var sportType = await _context.SportType.FindAsync(id);
            if (sportType == null)
            {
                return NotFound();
            }
            return View(sportType);
        }

        // POST: SportTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SportType sportType)
        {
            if (id != sportType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportTypeExists(sportType.Id))
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
            return View(sportType);
        }

        // GET: SportTypes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SportType == null)
            {
                return NotFound();
            }

            var sportType = await _context.SportType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportType == null)
            {
                return NotFound();
            }

            return View(sportType);
        }

        // POST: SportTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SportType == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SportType'  is null.");
            }
            var sportType = await _context.SportType.FindAsync(id);
            if (sportType != null)
            {
                _context.SportType.Remove(sportType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportTypeExists(int id)
        {
          return (_context.SportType?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
