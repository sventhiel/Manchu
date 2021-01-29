using System;

namespace Manchu.Entities
{
    public class Visit
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTimeOffset Start { get; set; }
        public int Breaks { get; set; }
        public TimeSpan Position { get; set; }
        public DateTimeOffset End { get; set; }
        public bool Completed { get; set; }

        public Visit()
        {
            Start = DateTimeOffset.UtcNow;
            Position = TimeSpan.MinValue;
            End = DateTimeOffset.MinValue;
            Completed = false;
            Breaks = 0;
        }
    }
}