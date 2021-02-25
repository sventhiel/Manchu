using LiteDB;
using Manchu.Models;
using Manchu.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        public Guid Create(Guid code)
        {
            var patientService = new PatientService(_connectionString);
            var visitService = new VisitService(_connectionString);

            if (patientService.FindByCode(code) != null)
                return visitService.Create(code);

            return Guid.Empty;
        }

        [HttpPost]
        public bool Stop(Guid id)
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
        public bool Pause(Guid id)
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

        [HttpPost]
        public bool Update(Guid id, int position)
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

        public IActionResult Index(Guid code)
        {
            var visitService = new VisitService(_connectionString);

            var visits = visitService.FindByCode(code).ToList();

            var model = visits.Select(v => VisitGridItemModel.Convert(v));

            return View(model);
        }
    }
}