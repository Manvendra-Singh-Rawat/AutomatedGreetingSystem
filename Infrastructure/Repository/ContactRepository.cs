using AutomatedGreetingSystem.Application.Interfaces;
using AutomatedGreetingSystem.Domain.Entity;
using AutomatedGreetingSystem.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AutomatedGreetingSystem.Infrastructure.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly AutoGreetDbContext context;

        // Constructor
        public ContactRepository(AutoGreetDbContext context) => this.context = context;
        
        // POST
        public async Task<bool> AddNewContact(Contacts contacts)
        {
            await context.Contacts.AddAsync(contacts);
            int updatedFiles = await context.SaveChangesAsync();
            return updatedFiles > 0;
        }
        
        public async Task<bool> DeleteContactWithEmail(string email)
        {
            var deletedRows = await context.Contacts.Where(e => e.Email == email).ExecuteDeleteAsync();
            return deletedRows > 0;
        }

        // GET
        public async Task<List<Contacts>> GetAllContacts()
        {
            var data = await context.Contacts.ToListAsync();
            return data;
        }
    }
}
