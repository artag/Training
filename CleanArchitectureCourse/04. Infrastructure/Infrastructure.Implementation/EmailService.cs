using Infrastructure.Interfaces.Integrations;

namespace Infrastructure.Implementation
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(string address, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}
