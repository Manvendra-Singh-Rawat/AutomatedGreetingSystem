using AutomatedGreetingSystem.Application.DTO;
using AutomatedGreetingSystem.Application.Interfaces;
using AutomatedGreetingSystem.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedGreetingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository) => _eventRepository = eventRepository;

        [HttpPost("addevent")]
        public async Task<ActionResult> AddNewEvent(CreateEventDTO eventDTO)
        {
            Events newEvent = new Events
            {
                EventName = eventDTO.EventName,
                DateOfEvent = DateOnly.FromDateTime(eventDTO.DateOfEvent),
                IsSent = false
            };
            await _eventRepository.AddNewEvent(newEvent);
            return Ok();
        }

        [HttpGet("getevent")]
        public async Task<ActionResult> GetAllEvents()
        {
            await _eventRepository.GetAllEvents();
            return Ok();
        }

        [HttpPut("updateevent")]
        public async Task<ActionResult> UpdateEventById(Events updateEvent)
        {
            bool isUpdated = await _eventRepository.UpdateEventById(updateEvent);
            return Ok();
        }

        [HttpDelete("deleteevent/{id}")]
        public async Task<ActionResult> DeleteEventById(int id)
        {
            await _eventRepository.DeleteEventById(id);
            return Ok();
        }
    }
}