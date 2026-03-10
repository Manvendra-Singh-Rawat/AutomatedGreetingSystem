using System.ComponentModel.DataAnnotations;

namespace AutomatedGreetingSystem.Application.DTO
{
    public class CreateContactDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
