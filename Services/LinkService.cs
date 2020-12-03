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

    }
}
