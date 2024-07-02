using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAwsEmailService
    {
        Task<string> SendEmailAsync(string email, string messageBody, string fname = "", string lname = "", string subject = "");
        Task<string> SendTemplatedConfirmEmailAsync(string toAddress, string userName, string callback = "");
        Task<string> SendTemplatedResetPasswordAsync(string toAddress, string userName, string callback = "");
        Task<string> SendTemplatedWelcomeEmailAsync(string toAddress, string userName, string callback = "");
        Task<bool> VerifyEmailIdentityAsync(string recipientEmailAddress);
    }
}
