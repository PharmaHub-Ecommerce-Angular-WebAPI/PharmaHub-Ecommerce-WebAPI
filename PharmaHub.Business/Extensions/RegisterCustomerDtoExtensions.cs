using PharmaHub.Domain.Entities.Identity;
using PharmaHub.DTOs.Authentication;

namespace PharmaHub.Business.Extensions;

public static class RegisterCustomerDtoExtensions
{
    public static Customer ToEntityCustomer(this CustomerDto dto)
          => new Customer
          {
              UserName= dto.CustomerName,
              Email = dto.Email,
              PhoneNumber= dto.PhoneNumber,
              Country = dto.Country,
              city = dto.city,
              PasswordHash = dto.Password,
          };
}
