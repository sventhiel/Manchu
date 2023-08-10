using Manchu.Entities;
using System;

namespace Manchu.Models
{
    public class CreatePatientModel
    {
        public Guid? Id { get; set; }
        public int? Number { get; set; }
    }

    public class PatientGridItemModel
    {
        public Guid Id { get; set; }

        public static PatientGridItemModel Convert(Patient patient)
        {
            return new PatientGridItemModel()
            {
                Id = patient.Id
            };
        }
    }

    public class ReadPatientModel
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public static ReadPatientModel Convert(Patient patient)
        {
            return new ReadPatientModel()
            {
                Id = patient.Id,
                Number = patient.Number
            };
        }
    }

    public class UpdatePatientModel
    {
        public Guid? Id { get; set; }

        public int? Number { get; set; }
    }
}