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

        public IActionResult Index(Guid id)
        {
            List<Patient> patients = null;

            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                patients = col.Query().ToList();
            }
            var url = $"{Request.Scheme}//{Request.Host.Value}/Home/Index";

            var model = patients.Select(p => PatientGridItemModel.Convert(p, url));

            return View(model);
        }

        public IActionResult Create()
        {
            using (var db = new LiteDatabase(_connectionString))
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