using AutomatedGreetingSystem.Application.DTO;
using AutomatedGreetingSystem.Application.Interfaces;
using AutomatedGreetingSystem.Domain.Entity;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AutomatedGreetingSystem.Application.Services
{
    public class GreetingService : IGreetingService
    {
        private readonly IContactRepository _contactRepo;
        private readonly IEventRepository _eventRepo;

        public GreetingService(IContactRepository contactRepo, IEventRepository eventRepo)
        {
            _contactRepo = contactRepo;
            _eventRepo = eventRepo;
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
            else
            {
                // Log
                Console.WriteLine("Events for today are not null");
            }

            var contactsList = await _contactRepo.GetAllContacts();
            if (contactsList.Count <= 0)
            {
                Console.WriteLine("Contacts list is empty");
                return null;
            }
            else
            {
                // Log
                Console.WriteLine("Contacts list not null");
            }

            string mailBody = GenerateEventMessage(todayEvents);
            Console.WriteLine($"Mail body: {mailBody}");
            return await SendMails(today, mailBody, contactsList);
        }

        private async Task<List<EndPointCheckerDTO>> SendMails(DateOnly todayDate, string mailBody, List<Contacts> contactsList)
        {
            var taskList = contactsList.Select(async contact =>
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("amanda37@ethereal.email", "XM8Bq9FAtPNqP76rPm");

                var email = new MimeMessage();
                email.From.Add(InternetAddress.Parse("amanda37@ethereal.email"));
                email.To.Add(InternetAddress.Parse(contact.Email));
                email.Subject = $"Event(s) on {todayDate}";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = mailBody };

                Console.WriteLine($"Sent to: {contact.Name} \nAt Mail: {contact.Email}\n\n");

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);

                return new EndPointCheckerDTO
                {
                    name = contact.Name,
                    email = contact.Email,
                };
            });

            var sendList = await Task.WhenAll(taskList);
            return sendList.ToList();
        }

        private string GenerateEventMessage(List<Events> eventsList) => string.Join("<br>", eventsList.Select(e => $"Event: {e.EventName}"));
    }
}
