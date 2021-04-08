using LiteDB;
using Manchu.Entities;
using Manchu.Models;
using Manchu.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manchu.Controllers
{
    public class PatientController : Controller
    {
        private readonly ConnectionString _connectionString;

        public PatientController(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public IActionResult Index()
        {
            List<Patient> patients = null;

            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                patients = col.Query().ToList();
            }
            var model = patients.Select(p => PatientGridItemModel.Convert(p));

            return View(model);
        }

        [HttpPost]
        public IActionResult Bunch(int amount)
        {
            var patientService = new PatientService(_connectionString);

            for (int i = 0; i < amount; i++)
            {
                patientService.Create();
            }

            return RedirectToAction("Index");
        }

        public IActionResult New()
        {
            var patientService = new PatientService(_connectionString);

            patientService.Create();

            return RedirectToAction("Index");
        }

        public IActionResult Create(Guid code)
        {
            if(code != null)
            {
                var patientService = new PatientService(_connectionString);

                patientService.Create(code);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Update()
        {
            return View();
        }
    }
}