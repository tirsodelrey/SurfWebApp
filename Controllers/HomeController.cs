using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SurfWebApp.Data;
using SurfWebApp.Models;

namespace SurfWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<SurfLog> surfLogs = new List<SurfLog>();
            List<SurfVideo> videos = new List<SurfVideo>();

            surfLogs = _context.SurfLogs.Where(x => x.Shared == true).OrderBy(x=>x.LogDate).ToList();
            videos = _context.SurfVideos.Where(x => x.Tag.Contains("Vídeos")).OrderBy(x => x.Date).ToList();

            /*
             List<FavSurfVideo> videos = _context....;

             LogsAndVideoViewModel logsAndVideo = new LogsAndVideoViewModel();
             logsAndVideo.SurfLogs = surfLogs;
             logsAndVideo.Videos = videos;

             return View(logsAndVideo);
             */

            logandSurfVideoViewModel logandSurfVideo = new logandSurfVideoViewModel();
            logandSurfVideo.Surflogs = surfLogs;
            logandSurfVideo.SurfVideos = videos;
            return View(logandSurfVideo);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [Authorize(Roles="admin")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
