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
        private ConnectionString connectionString;

        public PatientController(ConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        public IActionResult Index()
        {
            List<Patient> patients = null;

            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                patients = col.Query().ToList();
            }
            var url = Request.Scheme + "://" + Request.Host.Value;

            var model = patients.Select(p => PatientGridItemModel.Convert(p));

            return View(model);
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Create()
        {
            var p = new PatientService();

            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                // Create your new customer instance
                var patient = new Patient
                {
                    Name = DateTime.Now.ToString()
                };

                col.Insert(patient);
            }

            return RedirectToAction("Index");
        }
    }
}