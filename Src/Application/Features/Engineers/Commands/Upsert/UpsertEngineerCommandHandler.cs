using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;

namespace Coreapi.Application.Features.Engineers.Commands.Upsert
{
    public class UpsertEngineerCommandHandler : IRequestHandler<UpsertEngineerCommand, int>
    {
        private readonly IClientRepository clientRepository;
        private readonly ICurrentUser currentUser;
        private readonly IEngineerRepository engineerRepository;
        private readonly IUserManager userManager;
        private readonly ISmsService smsService;

        public UpsertEngineerCommandHandler(IClientRepository clientRepository, ICurrentUser currentUser, IEngineerRepository engineerRepository, IUserManager userManager, ISmsService smsService)
        {
            this.clientRepository = clientRepository;
            this.currentUser = currentUser;
            this.engineerRepository = engineerRepository;
            this.userManager = userManager;
            this.smsService = smsService;
        }
        public async Task<int> Handle(UpsertEngineerCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            if (request.IdSection == 0) throw new NotFoundException("شهر انتخاب نشده است");
            if (request.CellPhone.Length != 11)
                throw new NotFoundException("شماره موبایل صحیح نیست 11 رقمی با صفر");
            if (request.NaCode.Length != 10) throw new NotFoundException("کد ملی باید ده رقمی باشد");
            if (request.Id != null)
            {
                var engineer = await engineerRepository.GetById(Guid.Parse(request.Id));
                if (engineer is null) throw new NotFoundException("کارشناس برای ویرایش وجود ندارد");

                engineer.UpdateEngineer(
                    request.FullName, request.NaCode, request.CellPhone, 
                    Helper.ShamsiToMiladi(request.SolarBirthDate), 
                    request.SolarBirthDate, request.DadName, 
                    request.Tell, request.Address, request.IdSection, 
                    Helper.ShamsiToMiladi(request.SolarMembershipDate), 
                    request.SolarMembershipDate, request.FieldTypeEnum, 
                    request.EducationTypeEnum, request.MaritalStatusTypeEnum, 
                    request.RelatedTypeEnum, request.BankAccountNumber, 
                    request.Inactive, request.DefaultQuota, 
                    request.SortIndex, request.BankAccountBlocked, 
                    request.CertOfEarth,request.CertOfTest,
                    request.CertOfFiber,request.CertOfInspection, 
                    request.Has1Percent,
                    request.HasQuarterIncrease
                    );


                if (engineer.UserId != null)
                {
                    var user = await userManager.GetUserAsync(engineer.UserId);
                    await userManager.UpdateUser(engineer.UserId, engineer.CellPhone, !engineer.Inactive);

                    if (user != null && user.PhoneNumber != engineer.CellPhone)
                    {
                        var resultChange = await userManager.ResetDefaultPassword(user.UserName);
                        if (!resultChange) throw new NotAllowedException("رمز عبور قابل بازیابی نمی باشد");

                        await smsService.SendSmsCode(engineer.CellPhone, 7395, user.UserName,
                            "Abcd@" + engineer.CellPhone.Substring(engineer.CellPhone.Length-4,4));
                        
                    }
                }

            }
            else
            {

                var userFind = await userManager.GetUserByUserNameAsync(request.NaCode);
                var userId = "";
                if (userFind is null)
                {
        
                    var phone4 = request.CellPhone.Substring(request.CellPhone.Length - 4, 4);
                    userId = await userManager.CreateUserAsync(currentUser.ClientId, null, "Abcd@" + phone4, $"{request.NaCode}@kurdnezambargh.ir",
                        Coreapi.Common.Enums.CurrentUserTypeEnum.JWT, request.FullName, "Engineer", request.CellPhone,
                        false, request.FullName);
                    if (string.IsNullOrEmpty(userId)) { throw new NotFoundException("خطا در ایجاد کاربری"); }
                    await userManager.AddToRolesAsync(userId, new List<string>() { "Engineer" });
                }
                else
                {
                    userId = userFind.Id;
                }

                if (string.IsNullOrEmpty(userId)) { throw new NotFoundException("خطا در ایجاد کاربری"); }

                var engineer = new Engineer(userId,client, request.FullName, request.NaCode, request.CellPhone, Helper.ShamsiToMiladi(request.SolarBirthDate), request.SolarBirthDate, request.DadName, request.Tell, request.Address, request.IdSection, Helper.ShamsiToMiladi(request.SolarMembershipDate), request.SolarMembershipDate, request.FieldTypeEnum, request.EducationTypeEnum, request.MaritalStatusTypeEnum, request.RelatedTypeEnum, request.BankAccountNumber, request.Inactive, request.DefaultQuota, request.SortIndex, request.BankAccountBlocked, request.CertOfEarth, request.CertOfTest, request.CertOfFiber,request.CertOfInspection);

                engineerRepository.Add(engineer);

                await smsService.SendSmsCode(engineer.CellPhone, 7395, engineer.NaCode,
                    "Abcd@" + engineer.CellPhone.Substring(engineer.CellPhone.Length - 4, 4));
            }


            await engineerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
