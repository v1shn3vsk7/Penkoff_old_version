using System.Net;
using System.Net.Mail;

namespace SendingEmail;

public class Service
{
    private readonly ILogger<Service> _logger;

    public Service(ILogger<Service> logger)
    {
        _logger = logger;
    }
    public static void SendEmail(string receiver, int verificationCode)
    {
        try
        {
            MailMessage message = new();
            //message.IsBodyHtml = true;
            message.From = new MailAddress("penkoff.verificati0n@gmail.com", "Penkoff Verification");
            message.To.Add(receiver);
            message.Subject = "Verification";
            message.Body = "Your verification code is: " + verificationCode.ToString();

            using (SmtpClient client = new("smtp.gmail.com"))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("penkoff.verificati0n@gmail.com", "TinkoffASP.NetCore");
                client.Port = 587;
                client.EnableSsl = true;

                client.Send(message);
            }

        }
        catch(Exception e)
        {
            //_logger.LogError(e.GetBaseException().Message);
        }

    }

    public void SendEmailCustom()
    {

    }
}

