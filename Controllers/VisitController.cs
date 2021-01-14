using LiteDB;
using Manchu.Models;
using Manchu.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manchu.Controllers
{
    public class VisitController : Controller
    {
        private readonly ConnectionString _connectionString;

        public VisitController(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        [HttpPost]
        public Guid Create(Guid patientId)
        {
            var visitService = new VisitService(_connectionString);
            return visitService.Create(patientId);
        }

        [HttpPost]
        public bool Update(Guid id)
        {
            var visitService = new VisitService(_connectionString);
            return visitService.Update(id);
        }

        public IActionResult Index(Guid patientId)
        {
            var visitService = new VisitService(_connectionString);

            var visits = visitService.FindByPatientId(patientId).ToList();

            var model = visits.Select(v => VisitGridItemModel.Convert(v));

            return View(model);
        }
    }
}
