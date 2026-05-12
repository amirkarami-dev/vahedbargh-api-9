using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Identity.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenDto>
    {
        private readonly ISignInManager signInManager;
        private readonly IMapper mapper;

        public RefreshTokenCommandHandler(ISignInManager signInManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<RefreshTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var output = await signInManager.RefreshToken(request.UserId, request.RefreshToken, request.OneMinute);

            return mapper.Map<RefreshTokenDto>(output);
        }
    }
}
