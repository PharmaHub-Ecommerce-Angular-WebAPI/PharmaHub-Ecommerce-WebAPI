﻿using PharmaHub.Business.Contracts;
using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;
using PharmaHub.Domain.Objects;
using PharmaHub.DTOs;
using PharmaHub.DTOs.OderDTOs;


namespace PharmaHub.Business.Managers;
public class OrderManager : IOrderManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductManager _productManager;
    public OrderManager(IUnitOfWork unitOfWork, IProductManager productManager)
    {
        _unitOfWork = unitOfWork;
        _productManager = productManager;
    }

    public async Task<ProblemDetails?> CreateOrderAsync(CreateOrderDTOs orderDto)
    {
        // 1. Validate data
        if (orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            return new ProblemDetails("InvalidOrder", "Order must contain at least one product.");

        // 2. Validate and process each product
        foreach (var item in orderDto.OrderItems)
        {
            var problem = await _productManager.PurchasingProduct(item.ProductId, item.Quantity);
            if (problem != null)
                return problem;
        }

        // 3. Create Order
        var order = new Order
        {
            ID = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            PaymentMethod = orderDto.PaymentMethod,
            OrderStatus = OrderStatus.Pending,
            CustomerId = orderDto.CustomerId,
            ProductOrdersList = new List<ProductOrder>()
        };

        // 4. Build ProductOrders and update product quantities
        foreach (var item in orderDto.OrderItems)
        {
            var product = await _unitOfWork._productsRepo.GetIdAsync(item.ProductId);
            if (product == null)
                return new ProblemDetails("ProductNotFound", $"Product with ID {item.ProductId} not found.");

            product.Quantity -= item.Quantity;
            await _unitOfWork._productsRepo.UpdatedAsync(product);

            order.ProductOrdersList.Add(new ProductOrder
            {
                ProductId = item.ProductId,
                Amount = item.Quantity,
                OrderId = order.ID
            });
        }

        // 5. Save order
        await _unitOfWork._ordersRepo.AddAsync(order);
        await _unitOfWork.CompleteAsync();

        // 6. Return null to indicate success (no problem)
        return null;
    }


    public async Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid orderId)
    {
        var order = await _unitOfWork._ordersRepo.GetOrderWithDetailsAsync(orderId);
        return order != null
            ? MapToOrderDetailsDto(order)
            : throw new ($"Order with ID {orderId} not found");
    }

    private OrderDetailsDto MapToOrderDetailsDto(Order order)

        => new OrderDetailsDto(order)
        {
            ID = order.ID,
            CustomerName = order.Customer?.UserName ?? "Unknown",
            PaymentMethod = order.PaymentMethod, 
            OrderStatus = order.OrderStatus, 
            CreatedAt = order.OrderDate,
        };
    
    
}