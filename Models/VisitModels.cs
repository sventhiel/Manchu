using Manchu.Entities;
using System;

namespace Manchu.Models
{
    public class ReadVisitModel
    {
        public int Breaks { get; set; }
        public DateTimeOffset End { get; set; }
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public int Position { get; set; }
        public DateTimeOffset Start { get; set; }

        public static ReadVisitModel Convert(Visit visit)
        {
            return new ReadVisitModel()
            {
                Id = visit.Id,
                PatientId = visit.Patient.Id,
                Start = visit.Start,
                Breaks = visit.Breaks,
                Position = visit.Position,
                End = visit.End
            };
        }
    }

    public class VisitGridItemModel
    {
        public int Breaks { get; set; }
        public DateTimeOffset End { get; set; }
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public int Position { get; set; }
        public DateTimeOffset Start { get; set; }

        public static VisitGridItemModel Convert(Visit visit)
        {
            return new VisitGridItemModel()
            {
                Id = visit.Id,
                PatientId = visit.Patient.Id,
                Start = visit.Start,
                Position = visit.Position,
                Breaks = visit.Breaks,
                End = visit.End,
            };
        }
    }
}