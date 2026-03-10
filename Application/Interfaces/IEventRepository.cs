using AutomatedGreetingSystem.Domain.Entity;

namespace AutomatedGreetingSystem.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<bool> AddNewEvent(Events newEvent);
        Task<List<Events>> GetAllEvents();
        Task<List<Events>> GetEventsBydate(DateOnly date);
        Task<bool> UpdateEventById(Events updateEvent);
        Task DeleteEventById(int Id);
    }
}
