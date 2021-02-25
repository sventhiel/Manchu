using Manchu.Entities;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Manchu.Models
{
    public class CreatePatientModel
    {
        public string Name { get; set; }
        public string Reference { get; set; }
    }
    public class PatientGridItemModel
    {
        public Guid Code { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string QRCode { get; set; }

        public static PatientGridItemModel Convert(Patient patient, string url)
        {
            var base64 = "";

            //using (var ms = new MemoryStream())
            //{
            //    Bitmap baseImage = (Bitmap)Image.FromFile(@"~/media/images/manchu.png");

            //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //    QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{url}?id={patient.Id}", QRCodeGenerator.ECCLevel.Q);
            //    QRCode qrCode = new QRCode(qrCodeData);

            //    using (var bitmap = new Bitmap(qrCode.GetGraphic(20)))
            //    {
            //        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        base64 = System.Convert.ToBase64String(ms.GetBuffer()); //Get Base64
            //    }
            //}

            return new PatientGridItemModel()
            {
                Id = patient.Id,
                Code = patient.Code,
                Name = patient.Name,
                QRCode = base64
            };
        }
    }
}