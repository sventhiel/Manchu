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

    public class CreatePatientModel
    {
        public Guid? Code { get; set; }
        public int? Number { get; set; }
    }

    public class UpdatePatientModel
    {
        public int Id { get; set; }
        public Guid Code { get; set; }

        public int Number { get; set; }
    }

    public class ReadPatientModel
    {
        public int Id { get; set; }
        public Guid Code { get; set; }

        public int Number { get; set; }

        public static ReadPatientModel Convert(Patient patient)
        {
            return new ReadPatientModel()
            {
                Id = patient.Id,
                Code = patient.Code,
                Number = patient.Number
            };
        }
    }
}