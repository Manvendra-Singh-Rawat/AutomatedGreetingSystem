using AutomatedGreetingSystem.Application.DTO;

namespace AutomatedGreetingSystem.Application.Interfaces
{
    public interface IGreetingService
    {
        Task<List<EndPointCheckerDTO>> CheckAndSendGreet();
    }
}
