namespace AutomatedGreetingSystem.Application.DTO
{
    public class CreateEventDTO
    {
        public required string EventName { get; set; }
        public DateTime DateOfEvent { get; set; }
    }
}
