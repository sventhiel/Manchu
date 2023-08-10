using LiteDB;
using System;

namespace Manchu.Entities
{
    public class Visit
    {
        public Visit()
        {
            Start = DateTimeOffset.UtcNow;
            Position = 0;
            End = DateTimeOffset.MinValue;
            Breaks = 0;
        }

        public int Breaks { get; set; }
        public DateTimeOffset End { get; set; }
        public Guid Id { get; set; }

        [BsonRef("patients")]
        public Patient Patient { get; set; }

        public int Position { get; set; }
        public DateTimeOffset Start { get; set; }
    }
}