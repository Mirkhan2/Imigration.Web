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
