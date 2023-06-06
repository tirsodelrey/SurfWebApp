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
    public class SurfVideosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public SurfVideosController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SurfVideos
        public async Task<IActionResult> Index(string searchstring)
        {
            if (string.IsNullOrEmpty(searchstring))
            {
                searchstring = "";
            }

            return View(await _context.SurfVideos.OrderBy(x => x.Date).Where(y => y.Tag.Contains(searchstring)).ToListAsync());

        }

        // GET: SurfVideos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            List<FavSurfVideo> favs;
            if (user != null)
            {
                favs = _context.FavSurfVideos.Where(x => x.AppUser == user).ToList();
            }
            else
            {
                favs = new List<FavSurfVideo>();
            }
            ViewData["favs"] = favs;
            if (id == null)
            {
                return NotFound();
            }

            var surfVideo = await _context.SurfVideos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (surfVideo == null)
            {
                return NotFound();
            }

            return View(surfVideo);
        }

        // GET: SurfVideos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SurfVideos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Date,Title,Description,Url,Image,Caption,Tag")] SurfVideo surfVideo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(surfVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(surfVideo);
        }

        // GET: SurfVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surfVideo = await _context.SurfVideos.FindAsync(id);
            if (surfVideo == null)
            {
                return NotFound();
            }
            return View(surfVideo);
        }

        // POST: SurfVideos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Date,Title,Description,Url,Image,Caption,Tag")] SurfVideo surfVideo)
        {
            if (id != surfVideo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(surfVideo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurfVideoExists(surfVideo.ID))
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
            return View(surfVideo);
        }


        // GET: SurfVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surfVideo = await _context.SurfVideos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (surfVideo == null)
            {
                return NotFound();
            }

            return View(surfVideo);
        }

        // POST: SurfVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var surfVideo = await _context.SurfVideos.FindAsync(id);
            _context.SurfVideos.Remove(surfVideo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SurfVideoExists(int id)
        {
            return _context.SurfVideos.Any(e => e.ID == id);
        }

        public async Task<IActionResult> AddtoFav(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var user = _userManager.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            var film = await _context.SurfVideos.FirstOrDefaultAsync(m => m.ID == id);

            List<FavSurfVideo> favs = _context.FavSurfVideos.Where(x => x.AppUser == user).ToList();
            FavSurfVideo favSurfVideo = new FavSurfVideo();
            favSurfVideo.SurfVideo = film;
            favSurfVideo.AppUser = user;
            favSurfVideo.AddedOn = DateTime.Now;


            if (favs.Find(x => x.SurfVideo == film) == null)
            {
                _context.Add(favSurfVideo);

            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "FavSurfVideos");

            //film.Rented = true;
            // _context.Update(film);


        }
    }
}
