using LazZiya.ImageResize;
using LiteDB;
using Manchu.Entities;
using Manchu.Extensions;
using Manchu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using static System.Net.Mime.MediaTypeNames;

namespace Manchu.Controllers
{
    [ApiController, Route("api"), Authorize(Roles = "admin")]
    public class ImageController : ControllerBase
    {
        private readonly ConnectionString _connectionString;

        public ImageController(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        [HttpPost("images")]
        public IActionResult Create(bool numberAsWatermark)
        {
            var patientService = new PatientService(_connectionString);

            var ids = patientService.Query().Select(p => p.Id).ToList();

            foreach (var id in ids)
            {
                Create(id, numberAsWatermark);
            }

            return Ok("getan!");
        }

        [HttpPost("images/{id}")]
        public IActionResult Create(Guid id, bool numberAsWatermark)
        {
            var patientService = new PatientService(_connectionString);

            var patient = patientService.FindById(id);

            var iwmOps = new ImageWatermarkOptions
            {
                Location = TargetSpot.BottomMiddle,
                Margin = 550
            };

            var twmOps = new TextWatermarkOptions
            {
                Location = TargetSpot.BottomRight,
                FontSize = 32,
                FontName = "Lato",
                TextColor = Color.Black
            };

            using (var img = Image.FromFile("./wwwroot/media/images/manchu.png"))
            {
                var url = $"{Request.Scheme}://{Request.Host.Value}/Home/Index";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{url}?code={patient.Id}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                if (numberAsWatermark)
                {
                    img.AddImageWatermark(qrCode.GetGraphic(25, Color.Black, Color.Transparent, true), iwmOps)
                        .ScaleByWidth(500)
                        .AddTextWatermark($"{patient.Number}", twmOps)
                        .SaveAs($"./wwwroot/media/codes/{patient.Id}.png");
                }
                else
                {
                    img.AddImageWatermark(qrCode.GetGraphic(25, Color.Black, Color.Transparent, true), iwmOps)
                        .ScaleByWidth(500)
                        .SaveAs($"./wwwroot/media/codes/{patient.Id}.png");
                }
            }

            return Ok("getan!");
        }

        [HttpGet("images/{id}")]
        public IActionResult Get(Guid id)
        {
            FileInfo image = new FileInfo($"./wwwroot/media/codes/{id}.png");

            if (System.IO.File.Exists(image.FullName))
            {
                return File(System.IO.File.OpenRead(image.FullName), "application/octet-stream", $"{image.GetFileNameWithoutExtension()}{image.GetExtension()}");
            }
            return NotFound();
        }

        [HttpGet("images")]
        public IActionResult Get()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo($"./wwwroot/media/codes");
                FileInfo file = new FileInfo($"{directory.Parent.FullName}/codes.zip");

                if (file.Exists)
                    file.Delete();

                ZipFile.CreateFromDirectory(directory.FullName, file.FullName);

                if (System.IO.File.Exists(file.FullName))
                {
                    return File(System.IO.File.OpenRead(file.FullName), "application/octet-stream", $"{file.GetFileNameWithoutExtension()}{file.GetExtension()}");
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}