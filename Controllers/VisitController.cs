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
        public Guid Create(Guid patientId)
        {
            var patientService = new PatientService(_connectionString);
            var visitService = new VisitService(_connectionString);

            if (patientService.FindById(patientId) != null)
                return visitService.Create(patientId);

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

        public IActionResult Index(Guid patientId)
        {
            var visitService = new VisitService(_connectionString);

            var visits = visitService.FindByPatientId(patientId).ToList();

            var model = visits.Select(v => VisitGridItemModel.Convert(v));

            return View(model);
        }
    }
}