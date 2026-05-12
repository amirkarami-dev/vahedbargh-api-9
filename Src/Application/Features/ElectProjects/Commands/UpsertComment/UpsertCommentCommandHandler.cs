using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpsertComment;

public class UpsertCommentCommandHandler(
    IClientRepository clientRepository,
    IElectProjectRepository electProjectRepository,
    ICommentEngFormRepository commentEngFormRepository,
    IEngineerRepository engineerRepository,
    ICurrentUser currentUser,
    IElectProjectProcessRepository processRepository)
    : IRequestHandler<UpsertCommentCommand, string>
{
    public async Task<string> Handle(UpsertCommentCommand request, CancellationToken cancellationToken)
    {

        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        var eng = await engineerRepository.getByUserId(currentUser.UserId);
        if (eng is null) throw new NotFoundException("این کاربری کارشناس نیست");



        if (string.IsNullOrEmpty(request.ElectProjectId)) throw new NotFoundException("این پرونده وجود ندارد");
        var electProject = await electProjectRepository.GetById(Guid.Parse(request.ElectProjectId));
        if (electProject is null) throw new NotFoundException("این پرونده وجود ندارد");


        var epp = await processRepository.GetElectProjectProcessById(Guid.Parse(request.EppId));
        if (epp is null) throw new NotFoundException("این تخصیص وجود ندارد");

        if (!epp.Accepted) throw new NotFoundException("ابتدا باید پرونده را قبول کنید");



        //if (electProject.ProjectLevelEnum == ProjectLevelEnum.ApproveStage)
        //    throw new NotFoundException("این پرونده قبلا تایید شده است");

        var commentId = request.Id;
        if (!string.IsNullOrEmpty(request.DeleteId) && string.IsNullOrEmpty(commentId))
        {
            var resultDelete = await commentEngFormRepository.DeleteById(Guid.Parse(request.DeleteId));
            if (resultDelete is null) throw new NotFoundException("مشکل در حذف");
            commentId = resultDelete.Id.ToString();
        }
        else
        {
            if (string.IsNullOrEmpty(commentId))
            {
                var comment = new CommentEngForm(request.BranchingTypeEnum, request.FazNumberEnum, request.BranchingCount, request.Ampere,
                    request.Power, request.PowerSum, request.Des, electProject, eng);
                commentEngFormRepository.Add(comment);
                commentId = comment.Id.ToString();
            }
            else
            {
                var comment = await commentEngFormRepository.GetById(Guid.Parse(commentId));

                if (comment is null) throw new NotFoundException("فرم اعلام نظر یافت نشد");
                comment.UpdateComment(request.BranchingTypeEnum, request.FazNumberEnum, request.BranchingCount, request.Ampere,
                    request.Power, request.PowerSum, request.Des);
                commentEngFormRepository.Update(comment);
            }
        }
        

        await commentEngFormRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return commentId;
    }
}