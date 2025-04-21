using PharmaHub.Domain.Entities.Identity;
using PharmaHub.DTOs.Authentication;

namespace PharmaHub.Business.Extensions;

public static class RegisterPharmacyDtoExtensions
{
    public static Pharmacy ToEntityPharmacy(this PharmacyDto dto)
          => new Pharmacy
          {
              UserName = dto.PharmacyName,
              Email = dto.Email,
              PhoneNumber = dto.PhoneNumber,
              Country = dto.Country,
              city = dto.city,
              PasswordHash = dto.Password,
          };
}
