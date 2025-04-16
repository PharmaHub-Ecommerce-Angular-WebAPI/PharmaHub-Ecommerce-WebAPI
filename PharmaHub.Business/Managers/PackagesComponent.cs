using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.DAL.Repositories;

namespace PharmaHub.Business.Managers
{
    public class PackagesComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public PackagesComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        //View Related Components
        public async Task<IReadOnlyList<string>> GetRelatedComponents(Guid productId)
        {
            var components = await _unitOfWork._productsRepo.GetRelatedComponents(productId);
            return components;
        }




    }
}
