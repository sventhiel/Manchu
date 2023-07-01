using LazZiya.ImageResize;
using LiteDB;
using Manchu.Services;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Net.Http;

namespace Manchu.Controllers
{
    [ApiController, Route("api")]
    public class ImageController : ControllerBase
    {
        private readonly ConnectionString _connectionString;

        public ImageController(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        [HttpPost("QRCodes")]
        public IActionResult CreateQRCodes()
        {
            var patientService = new PatientService(_connectionString);

            var ids = patientService.Query().Select(p => p.Id).ToList();

            foreach (var id in ids)
            {
                CreateQRCode(id);
            }

            return Ok("getan!");
        }

        [HttpPost("QRCode/{id}")]
        public void CreateQRCode(int id)
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

            using (var img = Image.FromFile($"./wwwroot/media/images/manchu.png"))
            {
                var url = $"{Request.Scheme}://{Request.Host.Value}/Home/Index";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{url}?code={patient.Code}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                img.AddImageWatermark(qrCode.GetGraphic(25, Color.Black, Color.Transparent, true), iwmOps)
                    .ScaleByWidth(500)
                    .AddTextWatermark($"{patient.Id}", twmOps)
                    .SaveAs($"./wwwroot/media/codes/{patient.Code}.png");
            }
        }
    }
}