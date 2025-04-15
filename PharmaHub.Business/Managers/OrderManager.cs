using PharmaHub.DAL.Repositories;
using  PharmaHub.DTOs;


namespace PharmaHub.Business.Managers;

public class OrderManager
{
    private readonly IUnitOfWork _unitOfWork;
    public OrderManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Read> CreateOrderAsync(CreateOrderDTOs dto)

    => await _unitOfWork._ordersRepo.UpsertAsync(dto.Products);
}
