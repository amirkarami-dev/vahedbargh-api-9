using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Models;
using Coreapi.Common.RequestModel;
using Coreapi.Common.Utility;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.PanelMakerAgg;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetProjectsFullFilter
{
    public class GetProjectsFullFilterQueryHandler : IRequestHandler<GetElectProjectsFullFilterQuery, PaggingList<ElectProjectViewModel>>
    {
        private readonly IElectProjectRepository electProjectRepository;
        private readonly IPanelMakerRepository panelMakerRepository;
        private readonly ICurrentUser currentUser;
        private readonly IClientRepository clientRepository;
        private readonly IUserManager userManager;

        public GetProjectsFullFilterQueryHandler(IElectProjectRepository electProjectRepository, IPanelMakerRepository panelMakerRepository, IMapper mapper, ICurrentUser currentUser, IClientRepository clientRepository, IUserManager userManager, ICommentEngFormRepository commentEngFormRepository, ICheckListFormRepository checkListFormRepository, IElectProjectErtFormRepository electProjectErtFormRepository)
        {
            this.electProjectRepository = electProjectRepository;
            this.panelMakerRepository = panelMakerRepository;
            this.currentUser = currentUser;
            this.clientRepository = clientRepository;
            this.userManager = userManager;
        }
        public async Task<PaggingList<ElectProjectViewModel>> Handle(GetElectProjectsFullFilterQuery request, CancellationToken cancellationToken)
        {
          
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }
            var output = new PaggingList<ElectProjectViewModel>()
            {
                Data = [],
                TotalItems = 0
            };
            var getUser = await userManager.GetUserAsync(currentUser.UserId) ?? throw new NotAllowedException("کاربری مشکل دارد");
            var model = new ElectProjectsFullFilterModel(
                request.SearchValue,
                request.ParentId, 
                request.FileNumber,
                request.ElectRequestNumber,
                request.SolarRegisterDate, 
                request.IdSection,
                getUser.IdCity,
                request.FilterProjectLevel, 
                request.ProjectLevelEnum, 
                request.LandLordName, 
                request.IsStop,
                request.RelatedPermitFilter,
                request.FilterIsFilter,
                request.IsBuildingInspection,
                request.IsEarthSystem,
                request.IsTestAndDelivery,
                request.ElectProjectStatusEnum,
                currentUser.Role.Contains("Elect"),
                request.Page - 1, 
                request.PageSize);

            if (!request.IsBuildingInspection && !request.IsEarthSystem && !request.IsTestAndDelivery)
            {
                model.FilterIsFilter = false;
            }

            var panelMaker = await panelMakerRepository.GetByUserid(currentUser.UserId);



            var electProjects = await electProjectRepository.GetElectProjectsFullFilter(client.Id,panelMaker?.Id, getUser.IdSection, model);

        

            foreach (var electProject in electProjects.ElectProjectViewModel)
            {
                
                electProject.ProjectBalance = electProject.ProjectBalanceIn - electProject.ProjectBalanceOut;

                var encodeGuid = Helper.EncodeGuid(electProject.Id);
                var param = $"e={encodeGuid}&a={Math.Abs(electProject.ProjectBalance)}";
                electProject.ProjectBalanceLinkForPay = $"/tp?{param}";

                if(electProject.ElectProjectProcessViewModel  != null)
                {

                    foreach (var process in electProject.ElectProjectProcessViewModel)
                    {
                        process.InspectionStatusName = process.InspectionStatusEnum.GetDisplayName();
                        process.ProjectLevelName = process.ProjectLevelEnum.GetDisplayName();
                    }

                    if (electProject.ElectProjectProcessViewModel.Any())
                    {
                        electProject.SolarDateDeliverEngineer =
                            electProject.ElectProjectProcessViewModel.FirstOrDefault()!.SolarDateDeliverEngineer;
                    }
                }

                var epp = electProject.ElectProjectProcessViewModel.FirstOrDefault();
                if (epp != null)
                {
                    electProject.EngCurrentName = epp.EngName + "-" + epp.ProjectLevelName + "-" + epp.InspectionStatusName;

                }



                output.Data.Add(electProject);

            }

            output.TotalItems = electProjects.CountRow;

            return output;
        }
    }
}
