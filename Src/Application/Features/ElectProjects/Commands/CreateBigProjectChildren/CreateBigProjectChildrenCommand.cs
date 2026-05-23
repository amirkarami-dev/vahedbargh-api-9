using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.ErtTariffAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.CreateBigProjectChildren;

/// <summary>
/// Creates child (sub-project) records for a big project.
/// Used by both Upsert and Update (Administrator) flows.
/// Does NOT call SaveChangesAsync — the calling handler is responsible for saving.
/// Returns the total payment amount attributed to the children (payWithSms).
/// </summary>
public record CreateBigProjectChildrenCommand(
    ElectProject ElectProject,
    Client Client,
    BuildingTariff BuildingTariff,
    ErtTariff? ErtTariff,
    int ChildInspectionCount,
    int ChildErtCount,
    long StartFileNumber,
    int Area,
    int AreaAsBuilt,
    int NumberOfFloor,
    string Address,
    string PostalCode,
    string LandlordName,
    string LandlordNaCode,
    string LandlordPhoneNumber,
    string LicenseNumber,
    int IdSection,
    bool HasSupervision,
    bool IsEarthSystem,
    bool IsTestAndDelivery,
    bool PanelNeed,
    int FoundationElectrodeArea,
    bool IsNeedEb,
    bool HasRelatedPermit
) : IRequest<long>;
