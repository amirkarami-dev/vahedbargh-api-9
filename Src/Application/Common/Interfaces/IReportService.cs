using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using CheckListForm = Coreapi.Common.ViewModels.CheckListForm;
using CommentEngForm = Coreapi.Common.ViewModels.CommentEngForm;

namespace Coreapi.Application.Common.Interfaces;

public interface IReportService
{
    Task<string> GetApprovedCommentForm(byte[] assign,IEnumerable<CommentEngForm> commentEngForms, ElectProject electProject,Engineer engineer);
    Task<string> GetApprovedCheckListForm(byte[] assign, IEnumerable<CheckListForm> checkListEngForms, ElectProject electProject, Engineer engineer,long workPermitNum, string solarDeliverEng);
    Task<string> GetApprovedErtForm(byte[] assign, byte[] border, ElectProjectErtForm electProjectErtForm,
        Engineer engineer, string solarDeliverEng);
    Task<string> GetApprovedErtForm(byte[] assign, ElectProjectErtForm electProjectErtForm,
        Engineer engineer, string solarDeliverEng, byte[] crookyOfElectrode,long workPermitNum);

    Task<string> GetApprovedSentToElectForm(byte[] assign, ElectProject electProject,
        string engName, string sendToElect);

    Task<string> GetApprovedCommentFormBigProject(
        List<(Engineer engineer, byte[] assignImage)> engineerAssignPairs,
        IEnumerable<CommentEngForm> commentEngForms,
        ElectProject electProject);

    Task<string> GetApprovedCheckListFormBigProject(
        List<(Engineer engineer, byte[] assignImage)> engineerAssignPairs,
        IEnumerable<CheckListForm> checkListEngForms,
        ElectProject electProject,
        string workPermitNum);
}