using System;
using System.Linq;
using Manchu.Entities;

namespace Manchu.Services
{
    public interface ILinkService
    {
        Guid Create(string patientId);
        bool Delete(Guid id);
        bool Delete(string patientId);
        Link FindById(Guid id);
        Link FindByPatientId(string patientId);
        IQueryable<Link> Query();
    }

    public class LinkService : ILinkService
    {
        public Guid Create(string patientId)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string patientId)
        {
            throw new NotImplementedException();
        }

        public Link FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Link FindByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Link> Query()
        {
            throw new NotImplementedException();
        }
    }
}
