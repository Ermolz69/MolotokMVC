using Microsoft.AspNetCore.Mvc;
using MolotokMvc.Data;
using MolotokMvc.Models;
using System.Linq;
using System.Collections.Generic;

namespace MolotokMvc.Controllers
{
    public class ResumeController : Controller
    {
        private readonly MolotokDbContext _context;

        public ResumeController(MolotokDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Tags = _context.Tag.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Resume resume)
        {
            int? userId = HttpContext.Session.GetInt32("LoggedId");
            if (userId == null)
            {
                return View();
            }
            resume.UserId = (int)userId;
            resume.Status = "open";
            resume.CreatedAt = DateTime.Now;
            _context.Resume.Add(resume);
            _context.SaveChanges();
            return RedirectToAction("Index", "Users");
        }

        public IActionResult Details(int id)
        {
            var resume = _context.Resume.FirstOrDefault(r => r.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }
    }
}