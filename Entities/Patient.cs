using System;
using System.Collections.Generic;

namespace Manchu.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public bool IsActive { get; set; }
        public List<Visit> Visits { get; set; }

        public Patient()
        {
            IsActive = true;
            Visits = new List<Visit>();
        }
    }
}