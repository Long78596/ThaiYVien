﻿using System.Net.Mail;
using System.Net;

namespace ThaiYVien.Repository.Email
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("longkolp16@gmail.com", "kwxd uzjs cars mian")
            };

            return client.SendMailAsync(
                new MailMessage(from: "longkolp16@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
