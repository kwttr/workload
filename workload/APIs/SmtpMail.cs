using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace workload.APIs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MailMessage mailMsg = new MailMessage())
            {
                // API key
                string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

                // To
                mailMsg.To.Add(new MailAddress("to@example.com", "To Name"));

                // From
                mailMsg.From = new MailAddress("from@example.com", "From Name");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "subject";
                string text = "text body";
                string html = @"<p>html body</p>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                using (SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("apikey", apiKey);
                    smtpClient.Send(mailMsg);
                }
            }
        }
    }
}
