using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.PanelMakerAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.AddPanelMaker;

public class AddElectProjectPanelMakerCommandHandler:IRequestHandler<AddElectProjectPanelMakerCommand,string>
{
    private readonly ICurrentUser _currentUser;
    private readonly IClientRepository _clientRepository;
    private readonly IElectProjectRepository _electProjectRepository;
    private readonly IPanelMakerRepository _panelMakerRepository;
    private readonly ISmsService _msService;


    public AddElectProjectPanelMakerCommandHandler(ICurrentUser currentUser, IClientRepository clientRepository, IElectProjectRepository electProjectRepository, IPanelMakerRepository panelMakerRepository, ISmsService msService)
    {
        _currentUser = currentUser;
        _clientRepository = clientRepository;
        _electProjectRepository = electProjectRepository;
        _panelMakerRepository = panelMakerRepository;
        _msService = msService;
    }
    public async Task<string> Handle(AddElectProjectPanelMakerCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetById(Guid.Parse(_currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), _currentUser.ClientId);
        }
        var electProject = await _electProjectRepository.GetById(Guid.Parse(request.ElectProjectId));
        if (electProject is null) throw new NotFoundException(" پرونده ای  با این شماره وجود ندارد ");

        var panelMaker = await _panelMakerRepository.GetById(Guid.Parse(request.PanelMakerId));
        if (panelMaker is null) throw new NotFoundException("تابلوساز وجود ندارد");

        electProject.UpdatePanelMaker(panelMaker);

        var resultSms = await _msService.SendSms2Params(panelMaker.MobileNumber,8700,Helper.MiladiToShamsiForSms(DateTime.Now),electProject.FileNumber.ToString());
        if (resultSms.Status != "success") throw new NotFoundException("پیام برای تابلوساز ارسال نشد");

        await _electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return panelMaker.Id.ToString();
    }
}