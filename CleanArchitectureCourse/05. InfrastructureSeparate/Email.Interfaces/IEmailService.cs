namespace Email.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string address, string subject, string body);
    }
}
