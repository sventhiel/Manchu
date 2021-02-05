using System;

namespace Manchu.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public bool IsActive { get; set; }

        public Patient()
        {
            Number = 0;
            IsActive = true;
        }
    }
}