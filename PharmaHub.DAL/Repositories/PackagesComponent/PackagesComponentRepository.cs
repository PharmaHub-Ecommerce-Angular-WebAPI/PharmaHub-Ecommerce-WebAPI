using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Infrastructure;

namespace PharmaHub.DAL.Repositories.PackagesComponent
{
    public class PackagesComponentRepository : GenericRepository<PharmaHub.Domain.Entities.PackagesComponent>, IPackagesComponentRepository
    {
        public PackagesComponentRepository(ApplicationDbContext context) : base(context) { }


        public async Task<List<String>> GetPackagesComponentsByProductIdAsync(Guid productId)
        {
            return await _dbSet
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.ComponentName)
                .ToListAsync();
        }

        public async Task AddComponents(ICollection<PharmaHub.Domain.Entities.PackagesComponent> components)
        {
             await _dbSet.AddRangeAsync(components);
        }

    }

}
