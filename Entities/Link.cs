using System;
using System.Collections.Generic;

namespace Manchu.Entities
{
    public class Link
    {
        public Guid Id { get; set; }
        public string PatientId { get; set; }
        public List<DateTimeOffset> Visits { get; set; }

        public Link()
        {
            Visits = new List<DateTimeOffset>();
        }
    }
}
