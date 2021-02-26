using LiteDB;
using Manchu.Entities;
using Manchu.Models;
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

        public IActionResult Bunch(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var col = db.GetCollection<Patient>("patients");

                    // Create your new customer instance
                    var patient = new Patient
                    {
                        Code = Guid.NewGuid()
                    };

                    col.Insert(patient);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View(new CreatePatientModel());
        }

        [HttpPost]
        public IActionResult Create(CreatePatientModel model)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                // Create your new customer instance
                var patient = new Patient
                {
                    Name = model.Name,
                    Reference = model.Reference,
                    Code = Guid.NewGuid()
                };

                col.Insert(patient);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(CreatePatientModel model)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                // Create your new customer instance
                var patient = new Patient
                {
                    Name = model.Name,
                    Reference = model.Reference,
                    Code = Guid.NewGuid()
                };

                col.Insert(patient);
            }

            return RedirectToAction("Index");
        }
    }
}