using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.UserFileAgg;
using MediatR;

namespace Coreapi.Application.Features.User.Queries.GetClientUserFiles
{
    public class GetClientUserFilesQueryHandler:IRequestHandler<GetClientUserFilesQuery,IEnumerable<GetUserFilesDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IClientRepository _clientRepository;
        private readonly IUserFileRepository _userFileRepository;

        public GetClientUserFilesQueryHandler(IMapper mapper, ICurrentUser currentUser, IClientRepository clientRepository, IUserFileRepository userFileRepository)
        {
            this._mapper = mapper;
            this._currentUser = currentUser;
            this._clientRepository = clientRepository;
            this._userFileRepository = userFileRepository;
        }
        public async Task<IEnumerable<GetUserFilesDto>> Handle(GetClientUserFilesQuery request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetWithSetting(Guid.Parse(_currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), _currentUser.ClientId);

            var usersFiles = await _userFileRepository.GetUserFilesByUserId(string.IsNullOrEmpty(request.UserId)?_currentUser.UserId:request.UserId);

            if (usersFiles is null) throw new NotFoundException("فایل وجود ندارد");

            return _mapper.Map<IEnumerable<GetUserFilesDto>>(usersFiles);
        }
    }
}
