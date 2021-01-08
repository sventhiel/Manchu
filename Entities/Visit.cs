using System;

namespace Manchu.Entities
{
    public class Visit
    {
        public Guid Id { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset Stop { get; set; }
        public bool WasCompleted { get; set; }

        public Visit()
        {
            Start = DateTimeOffset.UtcNow;
            Stop = DateTimeOffset.MinValue;
            WasCompleted = false;
        }
    }
}