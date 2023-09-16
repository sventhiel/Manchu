using LiteDB;
using Manchu.Models;
using Manchu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;

namespace Manchu.Controllers
{
    [ApiController, Route("api"), Authorize(Roles = "admin")]
    public class VisitController : ControllerBase
    {
        private readonly ConnectionString _connectionString;

        public VisitController(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        [HttpDelete("visits/{patientId}")]
        public IActionResult DeleteByPatientId(Guid patientId)
        {
            var visitService = new VisitService(_connectionString);
            var count = visitService.DeleteByPatientId(patientId);

            return Ok(count);
        }

        [HttpGet("visits")]
        public IActionResult Get()
        {
            var visitService = new VisitService(_connectionString);
            var visits = visitService.FindAll();

            return Ok(visits.Select(p => ReadVisitModel.Convert(p)));
        }

        [HttpGet("visits/{patientId}")]
        public IActionResult GetByPatientId(Guid patientId)
        {
            var visitService = new VisitService(_connectionString);
            var visits = visitService.FindByPatientId(patientId);

            return Ok(visits.Select(p => ReadVisitModel.Convert(p)));
        }
    }
}