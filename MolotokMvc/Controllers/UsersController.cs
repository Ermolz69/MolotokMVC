using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MolotokMvc.Data;
using MolotokMvc.Models;
using NuGet.Packaging;

namespace MolotokMvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly MolotokDbContext _context;
        private static List<Resume> resumes = new List<Resume>();
        private static List<Vacancy> vacancies = new List<Vacancy>();
        private static int selectedRow = 0;

        public UsersController(MolotokDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ProfileViewModel vm = new ProfileViewModel();
            User user = null;
            int? userId = HttpContext.Session.GetInt32("LoggedId");
            if (userId != null)
            {
                user = _context.User.Find(userId);
            }

            vm.User = user;

            if (user.Status == "company")
            {
                List<Vacancy> vacancies = _context.Vacancy.Where(v => v.UserId == userId)
                .OrderBy(v => v.CreatedAt)
                .ToList();
                vm.Vacancies = vacancies;
            }
            else if (user.Status == "user")
            {
                List<Resume> resumes = _context.Resume.Where(v => v.UserId == userId)
                .OrderBy(v => v.CreatedAt)
                .ToList();
                vm.Resumes = resumes;
            }

            vm.SelectRow = selectedRow;
            return View(vm);
        }

        public IActionResult PromoPage()
        {
            var userCount = _context.User.Count();
            var vacancyCount = _context.Vacancy.Count();
            var resumeCount = _context.Resume.Count();
            var companyCount = _context.User.Count(u => u.Status == "company");

            var promoData = new PromoViewModel
            {
                UserCount = userCount,
                VacancyCount = vacancyCount,
                ResumeCount = resumeCount,
                CompanyCount = companyCount
            };

            return View(promoData);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, IFormFile Avatar)
        {
            string base64 = string.Empty;
            if (Avatar != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Avatar.OpenReadStream().CopyTo(ms);
                    byte[] bytes = ms.ToArray();
                    base64 = Convert.ToBase64String(bytes);
                }
            }
            if (user != null)
            {
                user.Avatar = base64;
                user.CreatedAt = DateTime.Now.Date;
                _context.User.Add(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            User userFromDb = _context.User.Where(u => u.Email == Email &&
                u.Password == Password).FirstOrDefault();
            if (userFromDb != null)
            {
                HttpContext.Session.SetString("LoggedNick", userFromDb.Nick);
                HttpContext.Session.SetString("LoggedStatus", userFromDb.Status);
                HttpContext.Session.SetInt32("LoggedId", userFromDb.Id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult EditVacancy(int id)
        {
            Vacancy vacancy = _context.Vacancy.Find(id);
            if (vacancy == null)
            {
                return NotFound();
            }
            return PartialView("_EditVacancyPartial", vacancy);
        }

        [HttpGet]
        public IActionResult EditResume(int id)
        {
            Resume resume = _context.Resume.Find(id);
            if (resume == null)
            {
                return NotFound();
            }
            return PartialView("_EditResumePartial", resume);
        }


        [HttpPost]
        public IActionResult EditVacancy(Vacancy vacancy)
        {
            int? userId = HttpContext.Session.GetInt32("LoggedId");
            vacancy.UserId = (int)userId;
            if (userId == null )
            {
                ViewBag.Message = "You are not authorized to edit! \nuserId == null ";
                return View(vacancy);
            }

            if (string.IsNullOrEmpty(vacancy.Status))
            {
                vacancy.Status = "open";
            }

            _context.Vacancy.Update(vacancy);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditResume(Resume resume)
        {
            int? userId = HttpContext.Session.GetInt32("LoggedId");
            resume.UserId = (int)userId;
            if (userId == null)
            {
                ViewBag.Message = "You are not authorized to edit! \nuserId == null ";
                return View(resume);
            }

            if (string.IsNullOrEmpty(resume.Status))
            {
                resume.Status = "open";
            }

            _context.Resume.Update(resume);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Status(int id)
        {
            Vacancy vacancy = _context.Vacancy.Find(id);
            if (vacancy != null)
            {
                vacancy.Status = vacancy.Status == "open" ? "closed" : "open";
                _context.Vacancy.Update(vacancy);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public string StatusAjax(int id, string type)
        {
            string status = string.Empty;

            if (type == "vacancy")
            {
                var vacancy = _context.Vacancy.Find(id);
                if (vacancy != null)
                {
                    status = vacancy.Status == "open" ? "closed" : "open";
                    vacancy.Status = status;
                    _context.Vacancy.Update(vacancy);
                    _context.SaveChanges();
                }
            }
            else if (type == "resume")
            {
                var resume = _context.Resume.Find(id);
                if (resume != null)
                {
                    status = resume.Status == "open" ? "closed" : "open";
                    resume.Status = status;
                    _context.Resume.Update(resume);
                    _context.SaveChanges();
                }
            }

            return status;
        }


        [HttpGet("Users/FindVacancy/{id}")]
        public IActionResult FindVacancy(int id)
        {
            var resume = _context.Resume.Find(id);
            if (resume == null)
            {
                return NotFound();
            }

            string tags = resume.Tags;
            string[] tagList = tags.Split(", ");

            var vacancies = tagList
                .SelectMany(tag => _context.Vacancy
                    .Where(v => v.Tags.Contains(tag) && v.Status == "open"))
                .Distinct()
                .ToList();

            return PartialView("_VacancyListPartial", vacancies);
            
        }

        [HttpGet("Users/FindResumes/{id}")]
        public IActionResult FindResumes(int id)
        {
            var vacancy = _context.Vacancy.Find(id);
            if (vacancy == null)
            {
                return NotFound();
            }

            string tags = vacancy.Tags;
            string[] tagList = tags.Split(", ");

            var resumes = tagList
                .SelectMany(tag => _context.Resume
                    .Where(r => r.Tags.Contains(tag) && r.Status == "open"))
                .Distinct()
                .ToList();

            return PartialView("_ResumeListPartial", resumes);
        }
    }
}

