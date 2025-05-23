using EmployeesManagement.Data;
using EmployeesManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LeaveApplications.Include(l => l.Duration).Include(l => l.Employee).Include(l => l.LeaveType);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        public IActionResult Create()
        {
            ViewData["DurationId"] = new SelectList(_context.SystemCodesDetail, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,NoOfDays,StartDate,EndDate,DurationId,LeaveTypeId,Attachment,Description,StatusId,ApprovedById,ApprovedOn,CreatedById,CreatedOn,ModifiedById,ModifiedOn")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodesDetail, "Id", "Id", leaveApplication.DurationId);
            ViewData["StatusId"] = new SelectList(_context.Employees, "Id", "Id", leaveApplication.StatusId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Id", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodesDetail, "Id", "Id", leaveApplication.DurationId);
            ViewData["StatusId"] = new SelectList(_context.Employees, "Id", "Id", leaveApplication.StatusId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Id", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,NoOfDays,StartDate,EndDate,DurationId,LeaveTypeId,Attachment,Description,StatusId,ApprovedById,ApprovedOn,CreatedById,CreatedOn,ModifiedById,ModifiedOn")] LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
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
            ViewData["DurationId"] = new SelectList(_context.SystemCodesDetail, "Id", "Id", leaveApplication.DurationId);
            ViewData["StatusId"] = new SelectList(_context.Employees, "Id", "Id", leaveApplication.StatusId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Id", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }
    }
}
