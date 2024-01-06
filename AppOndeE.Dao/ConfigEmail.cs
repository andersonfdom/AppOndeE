using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace AppOndeE.Dao
{
    public class ConfigEmail
    {
        public string GetItemLabelValueEmail(string label, string value)
        {
            return $"<b style='font-size:18px'>{label}: </b><span style='font-size:15px;font-weight:normal'>{value}</span><br>";
        }

        public string GetBodyEmail(string HeaderEmail, string Subject, string ItensBody)
        {
            return "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                      $"<img src='{HeaderEmail}'>" +
                      $"<h2>{Subject}<h2><br>" +
                      $"<div style='text-align:left;word-wrap: break-word'>{ItensBody}</div><br /></div>";
        }

        public bool EmailEnviado(string emailDestino, string subject, string body, string tituloErro)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("ondee.app.br");


                mail.From = new MailAddress("contato@ondee.app.br");
                mail.To.Add(new MailAddress(emailDestino));
                mail.ReplyToList.Add(new MailAddress("contato@ondee.app.br"));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = false;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("contato@ondee.app.br", "OndeE@2023");

                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
