using System;

namespace Manchu.Entities
{
    public class Visit
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTimeOffset Start { get; set; }
        public int Breaks { get; set; }
        public int Position { get; set; }
        public DateTimeOffset End { get; set; }

        public Visit()
        {
            Start = DateTimeOffset.UtcNow;
            Position = 0;
            End = DateTimeOffset.MinValue;
            Breaks = 0;
        }
    }
}