using Microsoft.AspNetCore.Mvc;
using MolotokMvc.Data;
using MolotokMvc.Models;

namespace MolotokMvc.Controllers
{
    public class VacancyController : Controller
    {
        private readonly MolotokDbContext _context;

        public VacancyController(MolotokDbContext context)
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
        public IActionResult Create(Vacancy vacancy)
        {
            int? userId = HttpContext.Session.GetInt32("LoggedId");
            if(userId == null)
            {
                return View();
            }
            vacancy.UserId = (int)userId;
            vacancy.Status = "open";
            vacancy.CreatedAt = DateTime.Now;
            _context.Vacancy.Add(vacancy);
            _context.SaveChanges();
            return RedirectToAction("Index", "Users");
        }

        public IActionResult Details(int id)
        {
            var vacancy = _context.Vacancy.FirstOrDefault(v => v.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }
    }
}
