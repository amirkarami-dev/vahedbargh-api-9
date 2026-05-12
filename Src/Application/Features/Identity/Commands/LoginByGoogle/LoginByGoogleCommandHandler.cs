using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Identity.Commands.LoginByGoogle
{
    public class LoginByGoogleCommandHandler : IRequestHandler<LoginByGoogleCommand, LoginDto>
    {
        private readonly ISignInManager signInManager;
        private readonly IMapper mapper;

        public LoginByGoogleCommandHandler(ISignInManager signInManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<LoginDto> Handle(LoginByGoogleCommand request, CancellationToken cancellationToken)
        {
            return mapper.Map<LoginDto>(await signInManager.GoogleAuthenticate(request.TokenId));
        }
    }
}
