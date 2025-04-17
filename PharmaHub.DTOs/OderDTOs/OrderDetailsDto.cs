﻿using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.OderDTOs;

public record class OrderDetailsDto(
    Guid Id,
    string CustomerName,
    PaymentMethods PaymentMethod,
    OrderStatus OrderStatus,
    DateTime CreatedAt,
    decimal TotalAmount,
    List<ProductOrderDTOs> Products
    )
{  }
