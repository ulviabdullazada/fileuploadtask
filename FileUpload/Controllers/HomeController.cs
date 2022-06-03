using FileUpload.DAL;
using FileUpload.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context { get; }
        private IWebHostEnvironment _env { get; }

        public HomeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.Films.Include(f=>f.Images));
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Film film)
        {
            List<FilmImage> images = new List<FilmImage>();
            if (film.Files != null)
            {
                foreach (var item in film.Files)
                {
                    string filename = Guid.NewGuid().ToString() + item.FileName;
                    FilmImage image = new FilmImage {
                        ImageUrl = filename,
                        IsPoster = false,
                        Film = film
                    };
                    images.Add(image);
                    using (FileStream fs = new FileStream(Path.Combine(_env.WebRootPath,"images", filename),FileMode.Create))
                    {
                        item.CopyTo(fs);
                    }
                }
                film.Images = images;
            }
            string posterFileName = Guid.NewGuid().ToString() + film.CoverImage.FileName;
            FilmImage poster = new FilmImage
            {
                ImageUrl = posterFileName,
                IsPoster = true,
                Film = film
            };
            using (FileStream fs = new FileStream(Path.Combine(_env.WebRootPath, "images", posterFileName), FileMode.Create))
            {
                film.CoverImage.CopyTo(fs);
            }
            film.Images.Add(poster);
            _context.Films.Add(film);
            _context.SaveChanges();
            return View();
        }
        public IActionResult Details(int id)
        {
            Film film = _context.Films.Include(f => f.Images).FirstOrDefault(f => f.Id == id);
            if (film == null) return NotFound();
            return View(film);
        }
    }
}
