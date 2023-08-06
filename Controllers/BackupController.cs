using LiteDB;
using Manchu.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Manchu.Controllers
{
    [ApiController, Route("api"), Authorize(Roles = "admin")]
    public class BackupController : ControllerBase
    {
        private readonly ConnectionString _connectionString;

        public BackupController(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        [HttpGet("backups/database")]
        public IActionResult GetDatabase()
        {
            // database
            FileInfo database = new FileInfo(_connectionString.Filename);

            if (System.IO.File.Exists(database.FullName))
            {
                return File(System.IO.File.OpenRead(database.FullName), "application/octet-stream", $"{database.GetFileNameWithoutExtension()}_{DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss")}{database.GetExtension()}");
            }
            return NotFound();
        }
    }
}