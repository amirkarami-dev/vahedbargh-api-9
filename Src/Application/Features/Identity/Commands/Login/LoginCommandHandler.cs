using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Identity.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand,LoginDto>
    {
        private readonly ISignInManager signInManager;
        private readonly IMapper mapper;

        public LoginCommandHandler(ISignInManager signInManager,IMapper mapper) 
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var output = !request.OneMinute ? await signInManager.Login(request.UserName, request.Password, request.DeviceToken, request.PlatformType)
                : await signInManager.OneMinuteLogin(request.UserName,request.Password, request.DeviceToken,request.PlatformType);
           
            return mapper.Map<LoginDto>(output); 
        }
    }
}
