using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using MediatR;
using System;

namespace Coreapi.Application.Features.ElectProjects.Commands.UpsertErtForm;

public class UpsertErtFormCommand:IRequest<string>
{
    public string Id { get; set; }
    public string EppId { get; set; }


    // آدرس محل قرارگیری الکترود
    public string ElectrodeAddress { get; set; }
    // مشخصات جغرافیایی محل قرارگیری الکترود
    // utm: x
    public double UtmX { get; set; }
    // utm:y
    public double UtmY { get; set; }
    // تاریخ احداث الکترود
    public string ConstructionDate { get; set; }
    // نوع کاربری الکترود
    public ElectrodeUsageTypeEnum ElectrodeUsageTypeEnum { get; set; }
    public string ElectrodeUsageTypeOther { get; set; }
    // روش اجرای الکترود
    public ElectrodeExecuteTypeEnum ElectrodeExecuteTypeEnum { get; set; }
    public string ElectrodeExecuteTypeOther { get; set; }
    // نوع اکترود
    public ElectrodeTypeEnum ElectrodeTypeEnum { get; set; }
    public string ElectrodeTypeOther { get; set; }
    // جنس الکترود
    public ElectrodeMaterialTypeEnum ElectrodeMaterialTypeEnum { get; set; }
    public string ElectrodeMaterialTypeOther { get; set; }
    // طول الکترود
    public string ElectrodeLength { get; set; }
    // قطر الکترود
    public string ElectrodeDiameter { get; set; }
    // سایر اندازه گیری های الکترود
    public string OtherElectrodeMeasure { get; set; }
    public string Des { get; set; }
    // برند اقلام ارتینگ
    public string ErtBrand { get; set; }
    // برندو مدل ارت تستر
    public string ErtTesterBrand { get; set; }
    // تاریخ اندازه گیری
    public string MeasurementDate { get; set; }
    // ساعت اندازه گیری
    public string MeasurementHour { get; set; }
    // دمای هوا
    public string Temperature { get; set; }
    // مقدار بارندگی
    public string RainfallAmount { get; set; }
    //  مقاومت الکترود
    public string ElectrodeResistanceValue { get; set; }
    // روش اندازه گیری
    public string MeasurementMethod { get; set; }

    public long Amount { get; set; }

    public BuildingTypeEnum BuildingTypeEnum { get; set; }
}