using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoveMentor.Data;
using MoveMentor.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MoveMentor.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ContactsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var contacts = _context.Contacts
                .Where(c => c.UserId == userId);

            return View(await contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,mail")] Contacts contact)
        {
            contact.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,mail")] Contacts contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            contact.UserId = _userManager.GetUserId(User); // Upewnij się, że kontakt jest przypisany do tego samego użytkownika

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactsExists(contact.Id))
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
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contacts' is null.");
            }

            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactsExists(int id)
        {
            var userId = _userManager.GetUserId(User);
            return _context.Contacts?.Any(e => e.Id == id && e.UserId == userId) ?? false;
        }
    }
}
