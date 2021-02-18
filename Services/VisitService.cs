using LiteDB;
using Manchu.Entities;
using System;

namespace Manchu.Services
{
    public interface IVisitService
    {
        Guid Create(Guid code);

        bool Update(Visit visit);

        Visit FindById(Guid id);

        ILiteQueryable<Visit> FindByCode(Guid code);
    }

    public class VisitService : IVisitService
    {
        private readonly ConnectionString _connectionString;

        public VisitService(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public Guid Create(Guid code)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Visit>("visits");

                var visit = new Visit()
                {
                    Code = code,
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

        public ILiteQueryable<Visit> FindByCode(Guid code)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var col = db.GetCollection<Visit>("visits");

                var visits = col.Query().Where(p => p.Code == code);

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