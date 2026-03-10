using AutomatedGreetingSystem.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedGreetingSystem.Application.Interfaces
{
    public interface IContactRepository
    {
        // POST
        public Task<bool> AddNewContact(Contacts contacts);
        public Task<bool> DeleteContactWithEmail(string email);

        // GET
        public Task<List<Contacts>> GetAllContacts();
    }
}
