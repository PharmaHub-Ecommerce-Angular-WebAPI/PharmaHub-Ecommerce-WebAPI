using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Service.UserHandler.Verification;
public interface IEmailService
{
    Task SendVerificationCode(string email, string code, string username);
}
