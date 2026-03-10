using AutomatedGreetingSystem.Application.Interfaces;
using AutomatedGreetingSystem.Domain.Entity;
using AutomatedGreetingSystem.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AutomatedGreetingSystem.Infrastructure.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly AutoGreetDbContext _dbContext;

        public EventRepository(AutoGreetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddNewEvent(Events newEvent)
        {
            await _dbContext.Events.AddAsync(newEvent);
            int linesChanged = await _dbContext.SaveChangesAsync();
            return linesChanged > 0;
        }

        public async Task DeleteEventById(int Id)
        {
            await _dbContext.Events.Where(e => e.Id == Id).ExecuteDeleteAsync();
            return;
        }

        public async Task<List<Events>> GetAllEvents()
        {
            var data = await _dbContext.Events.ToListAsync();
            return data;
        }

        public async Task<List<Events>> GetEventsBydate(DateOnly date)
        {
            List<Events> todayEvents = await _dbContext.Events.Where(e => e.DateOfEvent == date).ToListAsync();
            return todayEvents;
        }

        public async Task<bool> UpdateEventById(Events updateEvent)
        {
            var eventEntity = await _dbContext.Events.FindAsync(updateEvent.Id);
            
            if (eventEntity == null) return false;

            eventEntity.DateOfEvent = updateEvent.DateOfEvent;
            eventEntity.EventName = updateEvent.EventName;
            int linesChanged = await _dbContext.SaveChangesAsync();

            return linesChanged > 0;
        }
    }
}
