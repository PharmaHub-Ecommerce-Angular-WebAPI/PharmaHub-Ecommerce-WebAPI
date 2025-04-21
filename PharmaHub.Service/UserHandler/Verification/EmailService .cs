using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Service.UserHandler.Verification;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
        _smtpClient = new SmtpClient
        {
            Host = _config["Smtp:Host"],
            Port = int.Parse(_config["Smtp:Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Smtp:Username"],
                _config["Smtp:Password"])
        };
    }

    public async Task SendVerificationCode(string email, string code, string username)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_config["Smtp:From"]),
            Subject = "PharmaHub – Your OTP Code for Account Verification",
            Body = $"Hello {username},\r\n\r\nThank you for registering with PharmaHub. To complete your account verification, please use the One-Time Password (OTP) below:\r\n\r\nYour OTP Code: {code}\r\n\r\nThis code is valid for the next 10 minutes. Please do not share it with anyone.\r\n\r\nBest regards,\r\nThe PharmaHub Team",
            IsBodyHtml = false
        };
        message.To.Add(email);
        _smtpClient.Send(message);
    }

}