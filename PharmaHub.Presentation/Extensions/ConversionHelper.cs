using PharmaHub.Presentation.ActionRequest.Account;

namespace PharmaHub.Presentation.Extensions;

public static class ConversionHelper
{
    public static RegisterPharmacyActionRequest ConvertToRegisterPharmacyActionRequest(PendingRegistration pendingRegistration)
    {
        return new RegisterPharmacyActionRequest
        {
            Email = pendingRegistration.Email,
            Password = pendingRegistration.Password, // Assuming you want to keep the password
            PharmacyName = pendingRegistration.PharmacyName,
            PhoneNumber = pendingRegistration.PhoneNumber,
            Country = pendingRegistration.Country,
            city = pendingRegistration.City,
            Address = pendingRegistration.Address,
            OpenTime = pendingRegistration.OpenTime,
            CloseTime = pendingRegistration.CloseTime
        };
    }
}
