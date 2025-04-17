using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.Authentication;
public class CustomerDto
{
    public CustomerDto(Customer user)
    {
        CustomerName= user.UserName;
        Email = user.Email;
        PhoneNumber= user.PhoneNumber;
        Country = user.Country;
        city = user.city;
    }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public CountriesSupported Country { get; set; }
    public Governorates city { get; set; }
}
