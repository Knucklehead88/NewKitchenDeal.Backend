using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Services
{
    public class AwsEmailService: IAwsEmailService
    {
        private readonly AmazonSimpleEmailServiceClient _amazonSimpleEmailServiceClient;

        public AwsEmailService()
        {
            _amazonSimpleEmailServiceClient = new AmazonSimpleEmailServiceClient("AKIA27DDRANT3GPQAPWQ", "xmCRJ2ZaBBVGd2Pr9NVkAzL504aMwGhJ9GWLpkcW", region: RegionEndpoint.USEast1);
        }

        public async Task<bool> VerifyEmailIdentityAsync(string recipientEmailAddress)
        {
            var success = false;
            try
            {
                var response = await _amazonSimpleEmailServiceClient.VerifyEmailIdentityAsync(
                    new VerifyEmailIdentityRequest
                    {
                        EmailAddress = recipientEmailAddress
                    });

                success = response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("VerifyEmailIdentityAsync failed with exception: " + ex.Message);
            }

            return success;
        }

        public async Task<string> SendEmailAsync(string email, string messageBody, string fname= "", string lname = "", string subject = "")
        {
            var messageId = "";
            string domain = email[email.IndexOf('@')..];
            try
            {
                var response = await _amazonSimpleEmailServiceClient.SendEmailAsync(
                    new SendEmailRequest
                    {
                        Destination = new Destination
                        {
                            ToAddresses = ["office@newprojectdeal.com"]
                        },
                        Message = new Message
                        {
                            Body = new Body
                            {
                                Text = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = messageBody
                                }
                            },
                            Subject = new Content
                            {
                                Charset = "UTF-8",
                                Data = $"{fname} {lname}"
                            }
                        },
                        Source = email
                    });

                messageId = response.MessageId;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return messageId;
        }

        public async Task<string> SendTemplatedWelcomeEmailAsync(string toAddress, string userName, string callback = "")
        {
            var messageId = "";
            try
            {
                var response = await _amazonSimpleEmailServiceClient.SendTemplatedEmailAsync(
                    new SendTemplatedEmailRequest
                    {
                        Source = "office@newprojectdeal.com",
                        Destination = new Destination
                        {
                            ToAddresses = [toAddress]
                        },
                        Template = "prod-welcomeUser",
                        TemplateData = $"{{ \"email\":\"{toAddress}\", \"logInUrl\": \"{callback}\" }}"
                    });
                messageId = response.MessageId;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return messageId;
        }

        public async Task<string> SendTemplatedConfirmEmailAsync(string toAddress, string userName, string callback = "")
        {
            var messageId = "";
            try
            {
                var response = await _amazonSimpleEmailServiceClient.SendTemplatedEmailAsync(
                    new SendTemplatedEmailRequest
                    {
                        Source = "office@newprojectdeal.com",
                        Destination = new Destination
                        {
                            ToAddresses = [toAddress]
                        },
                        Template = "prod-confirmAccount",
                        TemplateData = $"{{ \"email\":\"{toAddress}\", \"confirmAccountUrl\": \"{callback}\" }}"
                    });
                messageId = response.MessageId;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return messageId;
        }

        public async Task<string> SendTemplatedResetPasswordAsync(string toAddress, string userName, string callback = "")
        {
            var messageId = "";
            try
            {
                var response = await _amazonSimpleEmailServiceClient.SendTemplatedEmailAsync(
                    new SendTemplatedEmailRequest
                    {
                        Source = "office@newprojectdeal.com",
                        Destination = new Destination
                        {
                            ToAddresses = [toAddress]
                        },
                        Template = "prod-resetPassword",
                        TemplateData = $"{{ \"email\":\"{toAddress}\", \"resetPasswordUrl\": \"{callback}\" }}"
                    });
                messageId = response.MessageId;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return messageId;
        }
    }
}
