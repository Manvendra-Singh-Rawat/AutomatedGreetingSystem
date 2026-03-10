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

            var contactsList = await _contactRepo.GetAllContacts();
            if (contactsList.Count <= 0)
            {
                Console.WriteLine("Contacts list is empty");
                return null;
            }

            var sentList = await SendMails(contactsList, todayEvents);

            return sentList;
        }

        private async Task<List<EndPointCheckerDTO>> SendMails(List<Contacts> contactsList, List<Events> eventsList)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("amanda37@ethereal.email", "XM8Bq9FAtPNqP76rPm");

            List<EndPointCheckerDTO> sentList = new List<EndPointCheckerDTO>();

            foreach ( var contact in contactsList)
            {
                var email = new MimeMessage();
                email.From.Add(InternetAddress.Parse("amanda37@ethereal.email"));
                email.To.Add(InternetAddress.Parse(contact.Email));

                foreach(var events in eventsList)
                {
                    email.Subject = "Event: " + events.EventName;
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"testing email for event {events.EventName}" };
                    smtp.Send(email);
                }

                Console.WriteLine($"Mail sent to: {contact.Name} \nMail: {contact.Email}");
                sentList.Add(new EndPointCheckerDTO() { name = contact.Name, email = contact.Email });
            }

            await smtp.DisconnectAsync(true);
            return sentList;
        }
    }
}
