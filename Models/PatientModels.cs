using Manchu.Entities;
using System;

namespace Manchu.Models
{
    public class PatientGridItemModel
    {
        public Guid Code { get; set; }
        public int Id { get; set; }

        public static PatientGridItemModel Convert(Patient patient)
        {
            return new PatientGridItemModel()
            {
                Id = patient.Id,
                Code = patient.Code
            };
        }
    }
}