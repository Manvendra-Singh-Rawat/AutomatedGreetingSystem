using Microsoft.EntityFrameworkCore;

namespace AutomatedGreetingSystem.Domain.Entity
{
    [Index(nameof(Email), IsUnique = true)]
    public class Contacts
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
