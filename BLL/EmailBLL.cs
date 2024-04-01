using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace BLL
{
    public static class EmailBLL
    {
        // Methods
        public static void Enviar(string nome, string email, string mensagem, string subject)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            mail.From = new MailAddress("contato@sistemamix.com.br", "Contato - MiX");
            mail.To.Add("contato@sistemamix.com.br");
            mail.Subject = subject;
            mail.Body = mensagem;
            mail.Body = mail.Body + "<br /><br />" + nome;
            mail.Body = mail.Body + "<br />" + email;
            mail.IsBodyHtml = true;

            smtp.Send(mail);
        }
    }
}
