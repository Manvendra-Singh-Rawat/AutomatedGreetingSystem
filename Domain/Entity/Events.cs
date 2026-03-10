using Microsoft.EntityFrameworkCore;

namespace AutomatedGreetingSystem.Domain.Entity
{
    [Index(nameof(DateOfEvent))]
    public class Events
    {
        public int Id { get; private set; }
        public required string EventName { get; set; }
        public DateOnly DateOfEvent { get; set; }
        public bool IsSent { get; set; }
    }
}
