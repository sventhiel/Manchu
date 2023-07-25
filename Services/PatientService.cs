using LiteDB;
using Manchu.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manchu.Services
{
    public interface IPatientService
    {
        int? Create(Guid? code, int? number);

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

        public int? Create(Guid? code, int? number)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Patient>("patients");

                Patient patient = new Patient
                {
                    Code = code ?? Guid.NewGuid(),
                    Number = number ?? col.Max(x => x.Number) + 1
                };

                if (col.Exists(p => p.Code == code))
                    return null;

                if (col.Exists(p => p.Number == patient.Number))
                    return null;

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

                var patients = col.Find(p => p.Code == code);

                if (patients.Count() != 1)
                    return null;

                return patients.First();
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