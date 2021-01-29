using Manchu.Entities;
using System;

namespace Manchu.Models
{
    public class VisitGridItemModel
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTimeOffset Start { get; set; }
        public int Breaks { get; set; }
        public TimeSpan Position { get; set; }
        public DateTimeOffset End { get; set; }
        public bool Completed { get; set; }

        public static VisitGridItemModel Convert(Visit visit)
        {
            return new VisitGridItemModel()
            {
                Id = visit.Id,
                PatientId = visit.PatientId,
                Start = visit.Start,
                Position = visit.Position,
                Breaks = visit.Breaks,
                End = visit.End,
                Completed = visit.Completed
            };
        }
    }
}