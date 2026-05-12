using AutoMapper;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Identity.Commands.LoginByMac
{
    public class LoginByMacCommandHandler : IRequestHandler<LoginByMacCommand, LoginDto>
    {
        private readonly ISignInManager signInManager;
        private readonly IMapper mapper;

        public LoginByMacCommandHandler(ISignInManager signInManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<LoginDto> Handle(LoginByMacCommand request, CancellationToken cancellationToken)
        {
            var output = await signInManager.Login(request.UserName, request.Password, request.MacAddress);

            var result = mapper.Map<LoginDto>(output);

            result.Checksum = CreateMD5(string.Format("{0}+{1}+{2}", request.UserName, request.Password, request.MacAddress));

            return result;
        }

        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
