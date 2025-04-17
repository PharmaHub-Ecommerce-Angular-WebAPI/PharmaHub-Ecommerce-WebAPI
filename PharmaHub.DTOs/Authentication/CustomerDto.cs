using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.Authentication;
public class CustomerDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public CountriesSupported Country { get; set; }
    public Governorates city { get; set; }
}
public static class RegisterUserDtoExtensions
{
    public static Customer ToEntity(this CustomerDto dto)
        => new Customer
        {
            UserName = dto.CustomerName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = dto.Password,
            Country = dto.Country,
            city = dto.city,
        };
  

}
