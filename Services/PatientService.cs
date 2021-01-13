using LiteDB;
using Manchu.Entities;
using System;
using System.Linq;

namespace Manchu.Services
{
    public interface IPatientService
    {
        Guid Create(string reference);

        bool Delete(Guid id);

        Patient FindById(Guid id);

        Patient FindByReference(string reference);

        IQueryable<Patient> Query();
    }

    public class PatientService : IPatientService
    {
        private readonly ConnectionString _connectionString;

        public PatientService(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public Guid Create(string reference)
        {
            throw new NotImplementedException();
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

        public IQueryable<Patient> Query()
        {
            throw new NotImplementedException();
        }
    }
}