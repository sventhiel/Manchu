using LiteDB;
using Manchu.Entities;
using System;

namespace Manchu.Services
{
    public interface IPatientService
    {
        int Create();
        bool Delete(int id);

        Patient FindById(int id);

        Patient FindByCode(Guid code);

        ILiteQueryable<Patient> Query();
    }

    public class PatientService : IPatientService
    {
        private readonly ConnectionString _connectionString;

        public PatientService(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public int Create()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                Patient patient = new Patient
                {
                    Code = Guid.NewGuid()
                };

                var col = db.GetCollection<Patient>("patients");

                return col.Insert(patient);
            }
        }

        public int Create(Guid code)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                Patient patient = new Patient
                {
                    Code = code
                };

                var col = db.GetCollection<Patient>("patients");

                return col.Insert(patient);
            }
        }

        public bool Delete(int id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                return col.Delete(id);
            }
        }

        public Patient FindById(int id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                return col.FindById(id);
            }
        }

        public Patient FindByCode(Guid code)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                return col.FindOne(p => p.Code == code);
            }
        }

        public ILiteQueryable<Patient> Query()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                return col.Query();
            }
        }
    }
}