using LiteDB;
using Manchu.Models;
using Manchu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
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

        [HttpGet("patients")]
        public IActionResult Get()
        {
            var patientService = new PatientService(_connectionString);
            var patients = patientService.FindAll();

            return Ok(patients.Select(p => ReadPatientModel.Convert(p)));
        }

        [HttpGet("patients/{id}")]
        public IActionResult GetById(int id)
        {
            var patientService = new PatientService(_connectionString);
            var patient = patientService.FindById(id);

            return Ok(ReadPatientModel.Convert(patient));
        }

        [HttpPost("patients")]
        public IActionResult Post(CreatePatientModel model)
        {
            var patientService = new PatientService(_connectionString);

            var id = patientService.Create(model.Code, model.Number);

            if (id == null)
                return BadRequest("Oops, there was an issue.");

            return Ok($"The patient with id={id} has been created successfully.");
        }

        [HttpPut("patients/{id}/number")]
        public IActionResult Put(int id, int number)
        {
            var patientService = new PatientService(_connectionString);

            if (patientService.FindByNumber(number) != null)
                return BadRequest("Oops, there was an issue.");

            var patient = patientService.FindById(id);

            patient.Number = number;
            patientService.Update(patient);

            return Ok($"The patient with id={id} has been updated successfully.");
        }
    }
}