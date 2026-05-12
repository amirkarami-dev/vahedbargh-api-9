using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using MediatR;

namespace Coreapi.Application.Features.User.Queries.GetUserFile
{
    public class GetUserFileQueryHandler : IRequestHandler<GetUserFileQuery, byte[]>
    {
        private readonly IS3Service s3Service;

        public GetUserFileQueryHandler(IS3Service s3Service)
        {
            this.s3Service = s3Service;
        }
        public async Task<byte[]> Handle(GetUserFileQuery request, CancellationToken cancellationToken)
        {
            if (request.Path is null || string.IsNullOrEmpty(request.Path))
            {
                return System.Array.Empty<byte>();
            }

            return await s3Service.GetFileAttach(request.Path);
        }
    }
}