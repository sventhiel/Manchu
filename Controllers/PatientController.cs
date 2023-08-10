using LiteDB;
using Manchu.Models;
using Manchu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Manchu.Controllers
{
    [ApiController, Route("api"), Authorize(Roles = "admin")]
    public class PatientController : ControllerBase
    {
        private readonly ConnectionString _connectionString;

        public PatientController(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        [HttpDelete("patients/{id}")]
        public IActionResult DeleteById(Guid id)
        {
            var patientService = new PatientService(_connectionString);
            var result = patientService.Delete(id);

            return Ok($"The patient with id={id} has been deleted successfully.");
        }

        [HttpGet("patients")]
        public IActionResult Get()
        {
            var patientService = new PatientService(_connectionString);
            var patients = patientService.FindAll();

            return Ok(patients.Select(p => ReadPatientModel.Convert(p)));
        }

        [HttpGet("patients/{id}")]
        public IActionResult GetById(Guid id)
        {
            var patientService = new PatientService(_connectionString);
            var patient = patientService.FindById(id);

            return Ok(ReadPatientModel.Convert(patient));
        }

        [HttpPost("patients/bulk/{startNumber}/{amount}")]
        public IActionResult Post(int startNumber, int amount)
        {
            var patientService = new PatientService(_connectionString);

            List<int> created = new List<int>();

            for (int i = startNumber; i < startNumber + amount; i++)
            {
                var id = patientService.Create(null, i);

                if (id.HasValue)
                    created.Add(id.Value);
            }

            return Ok($"The patient(s) with numbers(s) ({string.Join(",", created)}) has/have been created successfully.");
        }

        [HttpPost("patients/{id}")]
        public IActionResult PostWithModel(Guid id, int? number)
        {
            var patientService = new PatientService(_connectionString);

            var result = patientService.Create(id, number);

            if (result == null)
                return BadRequest("Oops, there was an issue.");

            return Ok($"The patient with id={id} has been created successfully.");
        }

        [HttpPut("patients/{id}/{number}")]
        public IActionResult PutNumberById(Guid id, int number)
        {
            var patientService = new PatientService(_connectionString);

            if (patientService.FindByNumber(number) != null)
                return BadRequest("Oops, there was an issue.");

            var patient = patientService.FindById(id);

            patient.Number = number;
            patientService.Update(patient);

            return Ok($"The patient with id={id} has been updated successfully.");
        }

        [HttpPut("patients/number")]
        public IActionResult PutNumbers()
        {
            var patientService = new PatientService(_connectionString);
            var patients = patientService.FindAll().OrderBy(p => p.Id);

            var count = 0;

            foreach (var patient in patients)
            {
                patient.Number = ++count;
                patientService.Update(patient);
            }

            return Ok($"The number of the patients have been updated successfully.");
        }
    }
}