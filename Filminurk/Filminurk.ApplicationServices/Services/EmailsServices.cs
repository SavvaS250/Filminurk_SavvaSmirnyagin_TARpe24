using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Environment = Filminurk.Data.Environment;

namespace Filminurk.ApplicationServices.Services
{
    public class EmailsServices : IEmailsServices
    {
        private readonly IConfiguration _configuration;

        public EmailsServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(EmailDTO dto)
        {
            var email = new MimeMessage();
            _configuration.GetSection("EmailUserName").Value = Environment.gmailusername;
            _configuration.GetSection("EmailHost").Value = Environment.smtpaddress;
            _configuration.GetSection("EmailPassword").Value = Environment.gmailpass;

            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.SendToThisAdress));
            email.Subject = dto.EmailSubject;
            var builder = new BodyBuilder
            {
                HtmlBody = dto.EmailContent,
            };
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();

            smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
