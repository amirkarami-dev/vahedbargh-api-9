using MediatR;

namespace Coreapi.Application.Features.Notification.Commands.RequestDemo
{
    public class RequestDemoCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
    }
}
