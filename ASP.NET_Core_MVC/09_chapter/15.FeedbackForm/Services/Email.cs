using System.Net;
using System.Net.Mail;

namespace Feedback.Services;

public class Email : IEmail
{
    private readonly IConfiguration _settings;

    public Email(IConfiguration settings)
    {
        _settings = settings;
    }

    public string SendMessage(string reply)
    {
        var result = string.Empty;
        try
        {
            var message = new MailMessage();

            // Электронная почта отправителя.
            var mailSender = _settings["EmailSender"];
            message.From = new MailAddress(mailSender);

            // Электронная почта, на которую придет сообщение.
            // To - коллекция адресов получателей.
            var toMail = _settings["EmailTo"];
            message.To.Add(new MailAddress(toMail));

            // Тема сообщения.
            message.Subject = "От приложения Feedback поступило сообщение";
            // Тело сообщения.
            message.Body = reply;
            // Имеет ли текст почтового сообщения формат Html.
            message.IsBodyHtml = true;

            // Адрес SMTP-сервера, который осуществляет рассылку.
            var smtpServer = _settings["SmtpServer"];
            var smtp = new SmtpClient(smtpServer);
            var userName = _settings["UserMail"];
            var password = _settings["PwdMail"];
            smtp.Credentials = new NetworkCredential(userName, password);

            smtp.Send(message);
        }
        catch (Exception ex)
        {
            result = ex.Message;
        }

        return result;
    }
}
