using Core.Entities;
using Core.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public  class EmailService: IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string messageBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("NewProjectDeal", "office@newprojectdeal.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = messageBody };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_config["Email:Address"], _config["Email:Passcode"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task GetInTouchAsync(SendToEmail sendToEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress($"{sendToEmail.FirstName} {sendToEmail.LastName}", sendToEmail.Email));
            message.To.Add(new MailboxAddress("", "office@newprojectdeal.com"));
            message.Subject = "Get in Touch";
            message.Body = new TextPart("plain") { Text = sendToEmail.Message };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_config["Email:Address"], _config["Email:Passcode"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
