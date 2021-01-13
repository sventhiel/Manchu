using LiteDB;
using Manchu.Entities;
using System;

namespace Manchu.Services
{
    public interface IVisitService
    {
        Guid Create(Guid patientId);
        bool Update(Guid id);
        Visit FindById(Guid id);
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
                    Stop = DateTimeOffset.MinValue,
                    WasCompleted = false
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

        public bool Update(Guid id, bool complete = true)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Visit>("visits");

                var visit = FindById(id);

                if(visit != null)
                {
                    visit.Stop = DateTimeOffset.UtcNow;
                    visit.WasCompleted = complete;
                }

                return col.Update(visit);

            }
        }
    }
}