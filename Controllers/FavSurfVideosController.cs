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
    public class FavSurfVideosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public FavSurfVideosController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FavSurfVideos
        public async Task<IActionResult> Index(string searchstring)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (string.IsNullOrEmpty(searchstring))
            {
                searchstring = "";
            }

            return View(await _context.FavSurfVideos.Where(x => x.AppUser.Id == user.Id).Include(x =>x.SurfVideo).Where(y => y.SurfVideo.Tag.Contains(searchstring)).ToListAsync());
        }

        // GET: FavSurfVideos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favSurfVideo = await _context.FavSurfVideos.Include(x => x.SurfVideo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (favSurfVideo == null)
            {
                return NotFound();
            }

            return View(favSurfVideo);
        }

        // GET: FavSurfVideos/Create
        
        public IActionResult Create()
        {
            return View();
        }

        // POST: FavSurfVideos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AddedOn")] FavSurfVideo favSurfVideo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favSurfVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(favSurfVideo);
        }

        // GET: FavSurfVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favSurfVideo = await _context.FavSurfVideos.FindAsync(id);
            if (favSurfVideo == null)
            {
                return NotFound();
            }
            return View(favSurfVideo);
        }

        // POST: FavSurfVideos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AddedOn")] FavSurfVideo favSurfVideo)
        {
            if (id != favSurfVideo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favSurfVideo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavSurfVideoExists(favSurfVideo.ID))
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
            return View(favSurfVideo);
        }

        // GET: FavSurfVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favSurfVideo = await _context.FavSurfVideos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (favSurfVideo == null)
            {
                return NotFound();
            }

            return View(favSurfVideo);
        }

        // POST: FavSurfVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favSurfVideo = await _context.FavSurfVideos.FindAsync(id);
            _context.FavSurfVideos.Remove(favSurfVideo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavSurfVideoExists(int id)
        {
            return _context.FavSurfVideos.Any(e => e.ID == id);
        }
    }
}
