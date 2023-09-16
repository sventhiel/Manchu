using LiteDB;
using Manchu.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manchu.Services
{
    public interface IPatientService
    {
        Guid? Create(Guid? id, int? number);

        bool Delete(Guid id);

        Patient FindById(Guid id);

        Patient FindByNumber(int number);

        ILiteQueryable<Patient> Query();
    }

    public class PatientService : IPatientService
    {
        private readonly ConnectionString _connectionString;

        public PatientService(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public Guid? Create(Guid? id, int? number)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                Patient patient = new Patient
                {
                    Id = id ?? Guid.NewGuid(),
                    Number = number ?? col.Max(x => x.Number) + 1
                };

                if (col.Exists(p => p.Id == id))
                    return null;

                if (col.Exists(p => p.Number == patient.Number))
                    return null;

                return col.Insert(patient);
            }
        }

        public bool Delete(Guid id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var patients = db.GetCollection<Patient>("patients");
                var visits = db.GetCollection<Visit>("visits");

                var patient = patients.FindById(id);

                if (patient != null)
                {
                    visits.DeleteMany(v => v.PatientId == id);
                }

                return patients.Delete(id);
            }
        }

        public List<Patient> FindAll()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var patients = new List<Patient>();
                var col = db.GetCollection<Patient>("patients");

                foreach (var pat in col.FindAll())
                {
                    patients.Add(pat);
                }

                return patients;
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

        public Patient FindByNumber(int number)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                var patients = col.Find(p => p.Number == number);

                if (patients.Count() != 1)
                    return null;

                return patients.First();
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

        public bool Update(Patient patient)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                return col.Update(patient);
            }
        }
    }
}