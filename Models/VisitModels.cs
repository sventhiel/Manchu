using Manchu.Entities;
using System;

namespace Manchu.Models
{
    public class VisitGridItemModel
    {
        public Guid Id { get; set; }
        public Guid Code { get; set; }
        public DateTimeOffset Start { get; set; }
        public int Breaks { get; set; }
        public int Position { get; set; }
        public DateTimeOffset End { get; set; }

        public static VisitGridItemModel Convert(Visit visit)
        {
            return new VisitGridItemModel()
            {
                Id = visit.Id,
                Code = visit.Code,
                Start = visit.Start,
                Position = visit.Position,
                Breaks = visit.Breaks,
                End = visit.End,
            };
        }
    }
}