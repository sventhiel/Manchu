using Manchu.Entities;
using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace Manchu.Models
{
    public class PatientGridItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string QRCode { get; set; }

        public static PatientGridItemModel Convert(Patient patient)
        {
            var base64 = "";

            using (var ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"/{patient.Id}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (var bitmap = new Bitmap(qrCode.GetGraphic(20)))
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    base64 = System.Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                }
            }

            return new PatientGridItemModel()
            {
                Id = patient.Id,
                Name = patient.Name,
                QRCode = base64
            };
        }
    }
}