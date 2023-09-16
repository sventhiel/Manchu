using LiteDB;
using Manchu.Models;
using Manchu.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Manchu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConnectionString _connectionString;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ConnectionString connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        [HttpPost]
        public Guid CreateVisit(Guid code)
        {
            var patientService = new PatientService(_connectionString);
            var visitService = new VisitService(_connectionString);

            if (patientService.FindById(code) != null)
                return visitService.Create(code);

            return Guid.Empty;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index(Guid code)
        {
            var patientServive = new PatientService(_connectionString);

            if (patientServive.FindById(code) == null)
                return RedirectToAction("Info");

            return View(model: code);
        }

        public IActionResult Info()
        {
            return View();
        }

        [HttpPost]
        public bool PauseVisit(Guid id)
        {
            var visitService = new VisitService(_connectionString);

            var visit = visitService.FindById(id);

            if (visit != null)
            {
                visit.Breaks++;
                return visitService.Update(visit);
            }

            return false;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public bool StopVisit(Guid id)
        {
            var visitService = new VisitService(_connectionString);

            var visit = visitService.FindById(id);

            if (visit != null)
            {
                visit.End = DateTimeOffset.UtcNow;
                return visitService.Update(visit);
            }

            return false;
        }

        [HttpPost]
        public bool UpdateVisit(Guid id, int position)
        {
            var visitService = new VisitService(_connectionString);

            var visit = visitService.FindById(id);

            if (visit != null)
            {
                visit.Position = position;
                return visitService.Update(visit);
            }

            return false;
        }
    }
}