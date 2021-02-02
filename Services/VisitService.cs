using LiteDB;
using Manchu.Entities;
using System;

namespace Manchu.Services
{
    public interface IVisitService
    {
        Guid Create(Guid patientId);

        bool Update(Visit visit);

        Visit FindById(Guid id);

        ILiteQueryable<Visit> FindByPatientId(Guid patientId);
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

                var visit = new Visit()
                {
                    PatientId = patientId,
                    Start = DateTimeOffset.UtcNow,
                    End = DateTimeOffset.MinValue,
                };

                return col.Insert(visit);
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

        public ILiteQueryable<Visit> FindByPatientId(Guid patientId)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Visit>("visits");

                var visits = col.Query().Where(p => p.PatientId == patientId);

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