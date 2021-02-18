using System;

namespace Manchu.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }

        public Patient()
        {
            Code = Guid.NewGuid();
        }
    }
}