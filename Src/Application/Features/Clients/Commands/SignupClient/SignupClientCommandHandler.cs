using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;


namespace Coreapi.Application.Features.Clients.Commands.SignupClient
{
    public class SignupClientCommandHandler : IRequestHandler<SignupClientCommand, SignupOutput>
    {
        private readonly IClientRepository clientRepository;
        private readonly IUserManager userManager;
   
        private readonly IEmailService emailService;

        public SignupClientCommandHandler(IClientRepository clientRepository, IUserManager userManager
            , IEmailService emailService)
        {
            this.clientRepository = clientRepository;
            this.userManager = userManager;
           
            this.emailService = emailService;
        }
        public async Task<SignupOutput> Handle(SignupClientCommand request, CancellationToken cancellationToken)
        {
            if (await clientRepository.SubdomainExist(request.CompanyName.ToLower().Trim()))
                throw new BadRequestException("Subdomain Exist");

          

            if (await clientRepository.IsCardExist(request.CardNumber, request.Type))
                throw new BadRequestException("Card Number Exist");
            
            var userId = await userManager.CreateUserAsync(request.Email, request.Password, request.Email, request.FirstName, request.LastName);
            if (string.IsNullOrEmpty(userId))
            {
                string[] errors = { "Username or Password is Invalid" };
                throw new AuthenticationException(Errors: errors);
            }
            await userManager.AddToRolesAsync(userId, new List<string>() { "Administrator" });

            var client = new Client(request.CompanyName, request.ABN, request.CompanyType, request.StaffRange);


            client.AddCard(request.NameOnCard, request.CardNumber, request.Type, request.CCV, request.ExpireDate);

            clientRepository.Add(client);

            while (await clientRepository.ApiKeyExist(client.ApiKey))
                client.UpdateApiKey();

            var output = new SignupOutput();
           

           

            await clientRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await userManager.UpdateUser(userId, client.Id.ToString());

            var superusers = await userManager.GetUsers("SuperUser");
            foreach (var superuser in superusers)
            {
                await emailService.SendEmailAsync(superuser.Email, "New Client Registered"
                    , string.Format("{0} \n {1}",
                      $"Client {request.CompanyName} registered at Coreapi  at {DateTime.Now}."
                      , $"Selected package"));
            }

            return output;
        }
    }
}
