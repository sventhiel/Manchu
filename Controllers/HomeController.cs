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
        private readonly ILogger<HomeController> _logger;
        private readonly ConnectionString _connectionString;

        public HomeController(ILogger<HomeController> logger, ConnectionString connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public IActionResult Index(Guid id)
        {
            var patientServive = new PatientService(_connectionString);

            if (patientServive.FindById(id) == null)
                return RedirectToAction("Privacy");

            return View(model: id);
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