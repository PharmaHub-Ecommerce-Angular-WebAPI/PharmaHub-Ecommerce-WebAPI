using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.DTOs.ProductDTOs;

namespace PharmaHub.Business.Contracts
{
    public interface IFavoriteProductManager
    {

        public  Task AddFAVProduct(Guid productId, string cutomerId);
        public Task RemoveFAVProduct(Guid productId, string customerId);
        public Task<List<GetFAVProductDto>> GetAllFAVProducts(string customerId);

    }
}
