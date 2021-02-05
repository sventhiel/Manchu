using LiteDB;
using Manchu.Entities;
using System;

namespace Manchu.Services
{
    public interface IPatientService
    {
        Guid Create(string name, string reference);

        bool Delete(Guid id);

        Patient FindById(Guid id);

        Patient FindByReference(string reference);

        ILiteQueryable<Patient> Query();
    }

    public class PatientService : IPatientService
    {
        private readonly ConnectionString _connectionString;

        public PatientService(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public Guid Create(string name = "", string reference = "")
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                Patient patient = new Patient
                {
                    Name = name,
                    Reference = reference,
                };

                var col = db.GetCollection<Patient>("patients");

                return col.Insert(patient);
            }
        }

        public bool Delete(Guid id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                return col.Delete(id);
            }
        }

        public Patient FindById(Guid id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                return col.FindById(id);
            }
        }

        public Patient FindByReference(string reference)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                return col.FindOne(p => p.Reference == reference);
            }
        }

        public ILiteQueryable<Patient> Query()
        {
            throw new NotImplementedException();
        }
    }
}