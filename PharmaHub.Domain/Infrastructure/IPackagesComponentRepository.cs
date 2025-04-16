using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;

namespace PharmaHub.Domain.Infrastructure
{
    public interface IPackagesComponentRepository:IGenericRepository<PackagesComponent>
    {
        public Task<List<string>> GetPackagesComponentsByProductIdAsync(Guid productId);
        public Task AddComponents(ICollection<PharmaHub.Domain.Entities.PackagesComponent> components);
    }
}
