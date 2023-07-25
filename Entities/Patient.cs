using System;

namespace Manchu.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public int Number { get; set; }
    }
}