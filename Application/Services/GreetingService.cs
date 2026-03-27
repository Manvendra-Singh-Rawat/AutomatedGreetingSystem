using AutomatedGreetingSystem.Application.DTO;
using AutomatedGreetingSystem.Application.Interfaces;
using AutomatedGreetingSystem.Domain.Entity;
using AutomatedGreetingSystem.Infrastructure.Environment;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AutomatedGreetingSystem.Application.Services
{
    public class GreetingService : IGreetingService
    {
        private readonly IContactRepository _contactRepo;
        private readonly IEventRepository _eventRepo;
        private readonly SMTPSettings _smtpSettings;

        public GreetingService(IContactRepository contactRepo, IEventRepository eventRepo, IOptions<SMTPSettings> smtpSettings)
        {
            _contactRepo = contactRepo;
            _eventRepo = eventRepo;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<List<EndPointCheckerDTO>> CheckAndSendGreet()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            Console.WriteLine($"Todays date: {today}");

            List<Events> todayEvents = await _eventRepo.GetEventsBydate(today);
            if (todayEvents.Count <= 0)
            {
                Console.WriteLine($"No events on for the day");
                return null;
            }

            var contactsList = await _contactRepo.GetAllContacts();
            if (contactsList.Count <= 0)
            {
                Console.WriteLine("Contacts list is empty");
                return null;
            }

            string mailBody = GenerateEventMessage(todayEvents);

            var newTask = Task.Run(async () => await SendMails(today, mailBody, contactsList));

            List<EndPointCheckerDTO> result = new List<EndPointCheckerDTO>();
            foreach(var contact in contactsList)
            {
                result.Add(new EndPointCheckerDTO
                {
                    name = contact.Name,
                    email = contact.Email
                });
            }

            return result;
        }

        private async Task SendMails(DateOnly todayDate, string mailBody, List<Contacts> contactsList)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.Email, _smtpSettings.Password);            

            foreach (var contact in contactsList)
            {
                var email = new MimeMessage();
                email.From.Add(InternetAddress.Parse(_smtpSettings.Email));
                email.To.Add(InternetAddress.Parse(contact.Email));
                email.Subject = $"Event(s) on {todayDate}";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = mailBody };

                Console.WriteLine($"Sent to: {contact.Name} \nAt Mail: {contact.Email}\n\n");

                await smtp.SendAsync(email);
                Console.WriteLine("After sending");
                await smtp.DisconnectAsync(true);
                Console.WriteLine("After disconnect");
            }
            return;
        }

        private string GenerateEventMessage(List<Events> eventsList) => string.Join("<br>", eventsList.Select(e => $"Event: {e.EventName}"));
    }
}
