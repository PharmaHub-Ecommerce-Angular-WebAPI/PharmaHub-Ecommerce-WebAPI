using PharmaHub.
namespace PharmaHub.Service.Services;

public interface ITokenService
{
    Task<string> CreateToken(User user, DateTime expiryDate);
}
