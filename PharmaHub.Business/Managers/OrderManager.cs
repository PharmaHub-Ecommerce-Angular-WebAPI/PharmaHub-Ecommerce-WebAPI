using PharmaHub.DAL.Repositories;
using PharmaHub.DTOs.OderDTOs;


namespace PharmaHub.Business.Managers;
public class OrderManager
{
    private readonly IUnitOfWork _unitOfWork;
    public OrderManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReadOrderDto> CreateOrderAsync(CreateOrderDTOs dto)
    {

    }
}
