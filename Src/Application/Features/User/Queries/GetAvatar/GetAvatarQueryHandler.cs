using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.User.Queries.GetAvatar
{
    public class GetAvatarQueryHandler : IRequestHandler<GetAvatarQuery, byte[]>
    {
        private readonly IUserManager userManager;
        private readonly IFileService fileService;
        private readonly ICurrentUser currentUser;

        public GetAvatarQueryHandler(IUserManager userManager, IFileService fileService, ICurrentUser currentUser)
        {
            this.userManager = userManager;
            this.fileService = fileService;
            this.currentUser = currentUser;
        }

        public async Task<byte[]> Handle(GetAvatarQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserAsync(currentUser.UserId);
            if (user is null || string.IsNullOrEmpty(user.Avatar))
                return System.Array.Empty<byte>();

            return await fileService.GetFile(user.Avatar);
        }
    }
}
