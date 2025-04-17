using PharmaHub.Domain.Entities;

namespace PharmaHub.DTOs;

public record class ProductOrderDTOs(Guid ProductId, short Amount, string ProductName);

