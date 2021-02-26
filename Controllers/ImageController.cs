using LazZiya.ImageResize;
using LiteDB;
using Manchu.Services;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace Manchu.Controllers
{
    public class ImageController : Controller
    {
        private readonly ConnectionString _connectionString;

        public ImageController(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateQRCodes()
        {
            var patientService = new PatientService(_connectionString);

            var codes = patientService.Query().Select(p => p.Code).ToList();

            foreach (var code in codes)
            {
                CreateQRCode(code);
            }

            return RedirectToAction("Index", "Patient");
        }

        public void CreateQRCode(Guid code)
        {
            var patientService = new PatientService(_connectionString);

            var patient = patientService.FindByCode(code);

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
