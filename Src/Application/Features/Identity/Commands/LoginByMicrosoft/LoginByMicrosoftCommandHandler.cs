using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Identity.Commands.LoginByMicrosoft
{
    public class LoginByMicrosoftCommandHandler : IRequestHandler<LoginByMicrosoftCommand, LoginDto>
    {
        private readonly ISignInManager signInManager;
        private readonly IMapper mapper;

        public LoginByMicrosoftCommandHandler(ISignInManager signInManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<LoginDto> Handle(LoginByMicrosoftCommand request, CancellationToken cancellationToken)
        {
            return mapper.Map<LoginDto>(await signInManager.MicrosoftAuthenticate(request.TokenId));
        }
    }
}
