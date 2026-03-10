using AutomatedGreetingSystem.Application.DTO;
using AutomatedGreetingSystem.Application.Interfaces;
using AutomatedGreetingSystem.Domain.Entity;
using AutomatedGreetingSystem.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedGreetingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private AutoGreetDbContext _context;
        private readonly IContactRepository _contactRepository;

        public ContactController(AutoGreetDbContext context, IContactRepository repo)
        {
            _context = context;
            _contactRepository = repo;
        }

        [HttpPost("addcontact")]
        public async Task<ActionResult> AddNewContact(CreateContactDTO contactDTO)
        {
            Contacts contact = new Contacts
            {
                Name = contactDTO.Name,
                Email = contactDTO.Email
            };

            bool status = await _contactRepository.AddNewContact(contact);

            if(status)
                Console.WriteLine($"New contact added by the name: {contact.Name} and email: {contact.Email}");
            else
                Console.WriteLine($"Not able to add contact by the name: {contact.Name}");

            return status == true ? Created("api/addcontact", contact) : StatusCode(500);
        }

        [HttpPut("deletecontact/{email}")]
        public async Task<ActionResult> DeleteContact(string email)
        {
            await _contactRepository.DeleteContactWithEmail(email);
            return Ok();
        }

        [HttpGet("getcontact")]
        public async Task<ActionResult> GetAllContacts()
        {
            var data = await _contactRepository.GetAllContacts();
            return Ok(data);
        }
    }
}
