using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfWebApp.Data;
using SurfWebApp.Models;

namespace SurfWebApp.Controllers
{
    public class SurfLogsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SurfLogsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SurfLogs
        public async Task<IActionResult> Index()
        {

            //AppUser loggedUser = await _userManager.GetUserAsync(User.Identity.Name);
            AppUser user = _context.Users.Include(u => u.SurfLogs).Single(u => u.UserName == User.Identity.Name);
            return View(user.SurfLogs.OrderBy(x => x.LogDate).ToList());
        }
        public async Task<IActionResult> SharedSurfLogs()
        {

            //AppUser loggedUser = await _userManager.GetUserAsync(User.Identity.Name);
            List<SurfLog> surflogs = new List<SurfLog>();
            surflogs = await _context.SurfLogs.Where(x => x.Shared == true).OrderBy(x => x.LogDate).ToListAsync();
            return View(surflogs);

        }
        // GET: SurfLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surfLog = await _context.SurfLogs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (surfLog == null)
            {
                return NotFound();
            }

            return View(surfLog);
        }

        // GET: SurfLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SurfLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LogDate,Location,HowLong,Comment,Rating")] SurfLog surfLog)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.GetUserAsync(User);
                if(user.SurfLogs == null)
                {
                    user.SurfLogs = new List<SurfLog>();
                }
                user.SurfLogs.Add(surfLog);
                _context.Add(surfLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(surfLog);
        }

        // GET: SurfLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surfLog = await _context.SurfLogs.FindAsync(id);
            if (surfLog == null)
            {
                return NotFound();
            }
            return View(surfLog);
        }

        // POST: SurfLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LogDate,Location,HowLong,Comment,Rating")] SurfLog surfLog)
        {
            if (id != surfLog.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(surfLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurfLogExists(surfLog.ID))
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
            return View(surfLog);
        }

        // GET: SurfLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surfLog = await _context.SurfLogs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (surfLog == null)
            {
                return NotFound();
            }

            return View(surfLog);
        }

        // POST: SurfLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var surfLog = await _context.SurfLogs.FindAsync(id);
            _context.SurfLogs.Remove(surfLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task <IActionResult> Share(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var surflog = await _context.SurfLogs.FirstOrDefaultAsync(m => m.ID == id);

            surflog.Shared = true;
            _context.Update(surflog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private bool SurfLogExists(int id)
        {
            return _context.SurfLogs.Any(e => e.ID == id);
        }
    }
}
