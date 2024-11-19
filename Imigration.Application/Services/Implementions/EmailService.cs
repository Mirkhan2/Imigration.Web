using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Imigration.Application.Services.Interfaces;
using Imigration.Domains.Interfaces;

namespace Imigration.Application.Services.Implementions
{
    public class EmailService : IEmailService
    {
        #region Ctor
        private readonly ISiteSettingRepository _siteSettingRepository;
        public EmailService(ISiteSettingRepository siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        public async Task<bool> SendEmail(string to, string subject, string body)
        {
            //MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            //mail.From = new MailAddress("mirkhan.shams4@gmail.com", "BugFixer");
            //mail.To.Add(to);
            //mail.Subject = subject;
            //mail.Body = body;
            //mail.IsBodyHtml = true;

            //SmtpServer.Port = 587;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("mirkhan.shams4@gmail.com", "fuqijttnofjradmh");
            //SmtpServer.EnableSsl = true;

            //SmtpServer.Send(mail);

            try
            {
                var defaultSiteEmail = await _siteSettingRepository.GetDefaultEmail();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(defaultSiteEmail.SMTP);

                mail.From = new MailAddress(defaultSiteEmail.From, defaultSiteEmail.DisplayName);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                if (defaultSiteEmail.Port != 0)
                {
                    SmtpServer.Port = defaultSiteEmail.Port;
                    SmtpServer.EnableSsl = defaultSiteEmail.EnableSSL;
                }

                SmtpServer.Credentials = new System.Net.NetworkCredential(defaultSiteEmail.From, defaultSiteEmail.Password);
                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
        #endregion

    }
}
