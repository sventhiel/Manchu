using Manchu.Entities;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Manchu.Models
{
    public class PatientGridItemModel
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string QRCode { get; set; }

        public static PatientGridItemModel Convert(Patient patient, string url)
        {
            var base64 = "";

            using (var ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{url}?id={patient.Id}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (var bitmap = new Bitmap(qrCode.GetGraphic(20)))
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    base64 = System.Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                }

                using (Image image1 = System.Drawing.Image.FromFile("C:\\inetpub\\wwwroot\\iisstart.png"))
                using (Image image2 = System.Drawing.Image.FromFile("C:\\Projects\\github\\sventhiel\\Manchu\\wwwroot\\media\\images\\manchu.png"))
                using (Bitmap b = new Bitmap(image2.Width, image2.Height + image1.Height))
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.DrawImageUnscaled(image2, 0, 0);
                    g.DrawImageUnscaled(image1, 0, image2.Height);

                    b.Save("C:\\inetpub\\wwwroot\\test.png", ImageFormat.Png);
                }
            }

            return new PatientGridItemModel()
            {
                Id = patient.Id,
                Number = patient.Number,
                Name = patient.Name,
                QRCode = base64
            };
        }
    }
}