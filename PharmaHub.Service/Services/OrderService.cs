using PharmaHub.Dtos.

using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;


namespace PharmaHub;

public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;
    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReadOrderDto> CreateOrderAsync(CreateOrderDTOs dto)
    
    => await _unitOfWork._ordersRepo.UpsertAsync(dto.Products);


    
}
