using System;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using Coreapi.Common.Enums;
using Newtonsoft.Json;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.PanelMakerAgg;

namespace Coreapi.Application.Features.User.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand,string>
    {
        private readonly IUserManager userManager;
        private readonly IExecutorRepository executorRepository;
        private readonly IPanelMakerRepository panelMakerRepository;
        private readonly IClientRepository clientRepository;
        private readonly ICurrentUser currentUser;
        private readonly IEngineerRepository engineerRepository;
        private readonly IEngineerHistoryRepository engineerHistoryRepository;
        private readonly ISmsService smsService;


        public AddUserCommandHandler(IUserManager userManager, IExecutorRepository executorRepository, IPanelMakerRepository panelMakerRepository, IClientRepository clientRepository, ICurrentUser currentUser, IEngineerRepository engineerRepository, IEngineerHistoryRepository engineerHistoryRepository, ISmsService smsService)
        {
            this.userManager = userManager;
            this.executorRepository = executorRepository;
            this.panelMakerRepository = panelMakerRepository;
            this.clientRepository = clientRepository;
            this.currentUser = currentUser;
            this.engineerRepository = engineerRepository;
            this.engineerHistoryRepository = engineerHistoryRepository;
            this.smsService = smsService;
        }

        public class PersonEngineer
        {
            public string FullName
            { get; set; }
            public string NaCode
            { get; set; }
            public long WorkPermitNum
            { get; set; }
            public string Additional
            { get; set; }
            public EngineerGradeTypeEnum  EngineerGradeTypeEnum
            { get; set; }
            public FieldTypeEnum FieldTypeEnum
            { get; set; }
            public string SolarValidityDate
            { get; set; }
            public string Email
            { get; set; }
        }
        public class PersonExecutor
        {
            public string FullName
            { get; set; }
            public string CompanyName
            { get; set; }
            public string CellPhone
            { get; set; }
            public int IdSection
            {
                get;
                set;
            }
        }

        public class PersonPanelMaker
        {
            public string FullName { get; set; }
            public string CompanyName { get; set; }
            public string CellPhone { get; set; }
            public int IdSection { get; set; }
            public string NaCode { get; set; }
            public string CompanyCode { get; set; }
            public string Address { get; set; }
        }

        //    public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
        //    {

        //        string jsonFilePath = @"D:\test.json";

        //        string json = File.ReadAllText(jsonFilePath);
        //        var person = JsonConvert.DeserializeObject<List<Person>>(json);


        //        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        //        if (client is null)
        //        {
        //            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        //        }

        //        foreach (var engineer in person)
        //        {
        //            var naCode = engineer.NaCode.Length == 10 ? engineer.NaCode : "1111111111";

        //            var userFind = await userManager.GetUserByEmailAsync(engineer.Email);
        //            var userId = "";
        //            if (userFind is null)
        //            {
        //                userId = await userManager.CreateUserAsync(currentUser.ClientId, null, "Abcd@1234", engineer.Email,
        //                Coreapi.Common.Enums.CurrentUserTypeEnum.JWT, engineer.FullName, "Engineer", "09180000000",
        //                false, engineer.FullName);
        //            }
        //            else
        //            {
        //                userId = userFind.Id;
        //            }

        //            if(userId is null) { throw new NotFoundException("یوزر ایجاد نشد"); }
        //            var newUser = await userManager.GetUserByEmailAsync(engineer.Email);

        //            var engOld = await engineerRepository.getByUserId(userId);
        //            if (engOld is not null) { throw new NotFoundException("قبلا ایجاد شده کارشناس"); }

        //            var eng = new Engineer(userId, engineer.FullName, naCode, "09180000000", DateTime.Now, "", "", "", "", 10184, DateTime.Now, "", engineer.FieldTypeEnum, EducationTypeEnum.BachelorDegree, MaritalStatusTypeEnum.Married, RelatedTypeEnum.Related, client, 0, "", false);


        //            await userManager.AddToRolesAsync(userId, new List<string>() { "Engineer" });

        //            var permit = engineer.SolarValidityDate.Substring(0, 4) + "/" + engineer.SolarValidityDate.Substring(4, 2) + "/" + engineer.SolarValidityDate.Substring(6, 2);
        //            var julianPermit = Helper.ShamsiToMiladi(permit);
        //            var engHistory = new EngineerHistory(eng, Coreapi.Common.Enums.EngineerGradeTypeEnum.SeniorDegree, engineer.WorkPermitNum,julianPermit,permit,
        //                julianPermit, permit, true);

        //                engineerHistoryRepository.Add(engHistory);


        //        }


        //        await engineerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        //        //------------------------------------------------------------------------------------------------------



        //        return "ok";
        //}



        //public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
        //{
        //    // executor
        //    var jsonFilePath = @"D:\executor.json";

        //    var json = File.ReadAllText(jsonFilePath);
        //    var person = JsonConvert.DeserializeObject<List<PersonExecutor>>(json);


        //    var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        //    if (client is null)
        //    {
        //        throw new NotFoundException(nameof(Client), currentUser.ClientId);
        //    }

        //    var i = 0;
        //    var j = 1000000000;
        //    foreach (var executor in person)
        //    {
        //        i++;
        //        j++;
        //        var userFind = await userManager.GetUserByEmailAsync($"executor{i}@kurdnezambargh.ir");
        //        var userId = "";
        //        if (userFind is null)
        //        {
        //            userId = await userManager.CreateUserAsync(currentUser.ClientId, null, "Abcd@1234", $"executor{i}@kurdnezambargh.ir",
        //            Coreapi.Common.Enums.CurrentUserTypeEnum.JWT, executor.FullName, "Executor", $"0{executor.CellPhone}",
        //            false, executor.FullName);
        //        }
        //        else
        //        {
        //            userId = userFind.Id;
        //        }

        //        if (userId is null) { throw new NotFoundException("یوزر ایجاد نشد"); }


        //        var newExecutor = new Executor(userId, OwnershipTypeEnum.Legal, ExecutorTypeEnum.Professional,
        //            ExecutorGradTypeEnum.Industrial, executor.CompanyName, executor.FullName
        //            , "", j.ToString(), $"0{executor.CellPhone}", true, "0", executor.IdSection, "", "", client, client.Id);


        //        await userManager.AddToRolesAsync(userId, new List<string>() { "Executor" });



        //        executorRepository.Add(newExecutor);


        //    }


        //    await executorRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        //    //------------------------------------------------------------------------------------------------------



        //    return "ok";
        //}

        //public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
        //{
        //    // executor
        //    var jsonFilePath = @"D:\panelMaker.json";

        //    var json = File.ReadAllText(jsonFilePath);
        //    var persons = JsonConvert.DeserializeObject<List<PersonPanelMaker>>(json);


        //    var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        //    if (client is null)
        //    {
        //        throw new NotFoundException(nameof(Client), currentUser.ClientId);
        //    }

        //    var i = 0;
        //    var j = 1000000000;
        //    foreach (var person in persons)
        //    {
        //        i++;
        //        j++;
        //        var userFind = await userManager.GetUserByEmailAsync($"{person.NaCode}@kurdnezambargh.ir");
        //        var userId = "";
        //        if (userFind is null)
        //        {


        //            var phone4 = person.CellPhone.Substring(person.CellPhone.Length - 4, 4);
        //            userId = await userManager.CreateUserAsync(currentUser.ClientId, null, "Abcd@" + phone4, $"user{person.CompanyCode}@kurdnezambargh.ir",
        //                CurrentUserTypeEnum.JWT, person.FullName, "PanelMaker", person.CellPhone,
        //                false, person.FullName);
        //            if (string.IsNullOrEmpty(userId)) { throw new NotFoundException("خطا در ایجاد کاربری"); }
        //            await userManager.AddToRolesAsync(userId, new List<string>() { "PanelMaker" });
        //        }
        //        else
        //        {
        //            userId = userFind.Id;
        //        }

        //        if (userId is null) { throw new NotFoundException("یوزر ایجاد نشد"); }


        //        var newPanelMaker = new PanelMaker(client,userId,person.NaCode,person.FullName
        //            , $"0{person.CellPhone}", true, person.CompanyName,person.CompanyCode,"0","","کردستان" 
        //            ,"سنندج",person.Address,person.IdSection,"","");


        //        await userManager.AddToRolesAsync(userId, new List<string>() { "PanelMaker" });



        //        panelMakerRepository.Add(newPanelMaker);


        //    }


        //    await panelMakerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        //    //------------------------------------------------------------------------------------------------------



        //    return "ok";
        //}


        public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            //Administrator
                   var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            if (request.UserName == null) return "username null";
            var userFind = await userManager.GetUserByEmailAsync($"{request.UserName}@kurdnezambargh.ir");
            var userId = "";
            if (userFind is null)
            {


                var phone4 = request.MobileNumber.Substring(request.MobileNumber.Length - 4, 4);
                userId = await userManager.CreateUserAsync(currentUser.ClientId, null, "Abcd@" + phone4, $"{request.UserName}@kurdnezambargh.ir",
                    CurrentUserTypeEnum.JWT, request.UserName, "Admin", request.MobileNumber,
                    false,request.UserName);
                if (string.IsNullOrEmpty(userId)) { throw new NotFoundException("خطا در ایجاد کاربری"); }
                await userManager.AddToRolesAsync(userId, new List<string>() { "Administrator" });
            }
            else
            {
                userId = userFind.Id;
            }

            if (userId is null) { throw new NotFoundException("یوزر ایجاد نشد"); }


            //------------------------------------------------------------------------------------------------------



            return "ok";
        }





    }
}
