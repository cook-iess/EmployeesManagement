using EmployeesManagement.Data;
using EmployeesManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Controllers
{
    public class SystemProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SystemProfiles.Include(s => s.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemProfile = await _context.SystemProfiles
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemProfile == null)
            {
                return NotFound();
            }

            return View(systemProfile);
        }

        public IActionResult Create()
        {
            ViewData["ProfileId"] = new SelectList(_context.SystemProfiles, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemProfile systemProfile)
        {
            //if (ModelState.IsValid)
            //{
            systemProfile.CreatedById = "Macro Code";
            systemProfile.CreatedOn = DateTime.Now;
            systemProfile.ModifiedById = "Macro Id";
            systemProfile.ModifiedOn = DateTime.Now;

            _context.Add(systemProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            ViewData["ProfileId"] = new SelectList(_context.SystemProfiles, "Id", "Name", systemProfile.ProfileId);
            return View(systemProfile);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemProfile = await _context.SystemProfiles.FindAsync(id);
            if (systemProfile == null)
            {
                return NotFound();
            }
            ViewData["ProfileId"] = new SelectList(_context.SystemProfiles, "Id", "Id", systemProfile.ProfileId);
            return View(systemProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SystemProfile systemProfile)
        {
            if (id != systemProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                systemProfile.ModifiedById = "Macro Id";
                systemProfile.ModifiedOn = DateTime.Now;

                try
                {
                    _context.Update(systemProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemProfileExists(systemProfile.Id))
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
            ViewData["ProfileId"] = new SelectList(_context.SystemProfiles, "Id", "Id", systemProfile.ProfileId);
            return View(systemProfile);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemProfile = await _context.SystemProfiles
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemProfile == null)
            {
                return NotFound();
            }

            return View(systemProfile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var systemProfile = await _context.SystemProfiles.FindAsync(id);
            if (systemProfile != null)
            {
                _context.SystemProfiles.Remove(systemProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemProfileExists(int id)
        {
            return _context.SystemProfiles.Any(e => e.Id == id);
        }
    }
}
