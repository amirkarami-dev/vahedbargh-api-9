using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.User.Commands.UpdateScore
{
    public class UpdateUserScoreCommand : IRequest
    {
        public string UserId { get; set; }
        public double Score { get; set; }
    }
}
