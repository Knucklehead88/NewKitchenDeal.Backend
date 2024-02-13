using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string messageBody);

        Task GetInTouchAsync(SendToEmail sendToEmail);
    }
}
