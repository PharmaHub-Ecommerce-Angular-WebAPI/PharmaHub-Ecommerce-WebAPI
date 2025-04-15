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

        public async Task<IReadOnlyList<Product>> GetLatestProductsAsync(int page, int size)
        {
            
        }

        #endregion
        

        #endregion



    }
}
