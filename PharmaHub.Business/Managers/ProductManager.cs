using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;

namespace PharmaHub.Business.Managers
{
    public class ProductManager
    {
        private IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Customer

        #region HomePage

        
        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _unitOfWork._productsRepo.GetAllAsync();
            return products.ToList();
        }

        #endregion


        #endregion



    }
}
