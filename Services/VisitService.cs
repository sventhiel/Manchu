using LiteDB;
using Manchu.Entities;
using System;
using System.Collections.Generic;

namespace Manchu.Services
{
    public interface IVisitService
    {
        Guid Create(Guid code);

        Visit FindById(Guid id);

        List<Visit> FindByPatientId(Guid patientId);

        bool Update(Visit visit);
    }

    public class VisitService : IVisitService
    {
        private readonly ConnectionString _connectionString;

        public VisitService(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public Guid Create(Guid patientId)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Visit>("visits");
                var patients = db.GetCollection<Patient>("patients");

                var visit = new Visit()
                {
                    PatientId = patientId,
                    Start = DateTimeOffset.UtcNow,
                    End = DateTimeOffset.MinValue,
                };

                return col.Insert(visit);
            }
        }

        public bool Delete(Guid id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var visits = db.GetCollection<Visit>("visits");

                return visits.Delete(id);
            }
        }

        public int DeleteByPatientId(Guid patientId)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var visits = db.GetCollection<Visit>("visits");

                return visits.DeleteMany(v => v.PatientId == patientId);
            }
        }

        public List<Visit> FindAll()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var visits = new List<Visit>();
                var col = db.GetCollection<Visit>("visits");

                foreach (var visit in col.FindAll())
                {
                    visits.Add(visit);
                }

                return visits;
            }
        }

        public Visit FindById(Guid id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Visit>("visits");

                return col.FindById(id);
            }
        }

        public List<Visit> FindByPatientId(Guid patientId)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Visit>("visits");

                var visits = col.Query().Where(v => v.PatientId == patientId).ToList();

                return visits;
            }
        }

        public bool Update(Visit visit)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Visit>("visits");

                if (visit != null)
                {
                    return col.Update(visit);
                }

                return false;
            }
        }
    }
}