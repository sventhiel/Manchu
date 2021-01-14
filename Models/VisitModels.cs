using Manchu.Entities;
using System;

namespace Manchu.Models
{
    public class VisitGridItemModel
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset Stop { get; set; }
        public bool Completed { get; set; }

        public static VisitGridItemModel Convert(Visit visit)
        {
            return new VisitGridItemModel()
            {
                Id = visit.Id,
                PatientId = visit.PatientId,
                Start = visit.Start,
                Stop = visit.Stop,
                Completed = visit.Completed
            };
        }
    }
}