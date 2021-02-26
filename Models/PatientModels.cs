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
        public string Reference { get; set; }

        public static PatientGridItemModel Convert(Patient patient, string url)
        {
            return new PatientGridItemModel()
            {
                Id = patient.Id,
                Code = patient.Code,
                Name = patient.Name,
                Reference = patient.Reference
            };
        }
    }
}