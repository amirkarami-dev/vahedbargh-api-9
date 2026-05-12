using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Models;
using Coreapi.Common.RequestModel;
using Coreapi.Common.Utility;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.ElectProjectProcesses.Queries.GetListProjectProcessEng
{
    public class GetListProjectProcessEngQueryHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IEngineerRepository engineerRepository,
        IElectProjectProcessRepository electProjectProcessRepository,
        IElectProjectRepository electProjectRepository,
        IClientRepository clientRepository,
        ICommentEngFormRepository commentEngFormRepository,
        ICheckListFormRepository checkListFormRepository,
        IElectProjectErtFormRepository electProjectErtFormRepository,
        ICheckListEdcRepository checkListEdcRepository,
        IUserManager userManager)
        : IRequestHandler<GetListProjectProcessEngQuery, PaggingList<ProjectProcessEngDto>>
    {
        public async Task<PaggingList<ProjectProcessEngDto>> Handle(GetListProjectProcessEngQuery request, CancellationToken cancellationToken)
        {

            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var engineer = await engineerRepository.getByUserId(currentUser.UserId);
            if (engineer is null && !currentUser.Role.Contains("Administrator") && !currentUser.Role.Contains("Section"))
            {
                throw new NotAllowedException("خطای مجوز دریافت رکورد");
            }
            var getUser = await userManager.GetUserAsync(currentUser.UserId) ?? throw new NotAllowedException("کاربری مشکل دارد");

            var output = new PaggingList<ProjectProcessEngDto>()
            {
                Data = new List<ProjectProcessEngDto>(),
                TotalItems = 0
            };

            var model = new EppFilterModel(request.SearchValue, request.FileNumber, request.SolarDateDeliverEngineer,request.LandlordName, request.IdSection, request.InspectionStatusEnum, request.Page - 1, request.PageSize,request.EngineerId);


            if (currentUser.Role.Contains("Administrator") || currentUser.Role.Contains("Section"))
            {
                if (currentUser.Role.Contains("Section")) model.IdSection = getUser.IdSection;
                var resultQuery = await electProjectProcessRepository.GetEppByClient(client.Id, model);
                output.Data = mapper.Map<List<ProjectProcessEngDto>>(resultQuery.AggregateModel);

                output.TotalItems = resultQuery.TotalItem;

            }
            else
            {
                var resultQuery = await electProjectProcessRepository.GetEppByEng(engineer.Id, model);
                var outputQuery = mapper.Map<List<ProjectProcessEngDto>>(resultQuery.AggregateModel);
                var getComments = await commentEngFormRepository.GetAllCommentEngForm(engineer.Id);
                var getCheckListForms = await checkListFormRepository.GetAllCheckListForm(engineer.Id);
                //var getErtForm = await electProjectErtFormRepository.GetAllErtForm();
                foreach (var qq in outputQuery)
                {
                    var getBalanceProject = await electProjectRepository.GetProjectBalance(qq.ElectProjectId);

                    var encodeGuid = Helper.EncodeGuid(qq.ElectProjectId);
                    var param = $"e={encodeGuid}&a={Math.Abs(getBalanceProject)}";
                    qq.ProjectBalanceLinkForPay = $"/tp?{param}";

                    qq.ProjectBalance = getBalanceProject;




                    qq.CommentEngForm = getComments.Where(f => f.ElectProject.Id == qq.ElectProjectId);
                    qq.CheckListForms = getCheckListForms.Where(f => f.ElectProject.Id == qq.ElectProjectId).OrderBy(o=>o.InspectionDesEnum);
                    qq.CheckListEdcs = await checkListEdcRepository.GetCheckListEdcForm(qq.ElectProjectId);
                    //qq.ElectProjectErtForm = getErtForm.FirstOrDefault(f => f.ElectProject.Id == qq.ElectProjectId);
                    qq.ElectProjectErtForm =
                        await electProjectErtFormRepository.GetEpeFormByElectProjectId(qq.ElectProjectId);
                };
                output.Data = outputQuery;

                output.TotalItems = resultQuery.TotalItem;
            }


            //if (output.TotalItems == 0) throw new NotFoundException("تخصیص وجود ندارد");


            return output;


        }
    }
}
