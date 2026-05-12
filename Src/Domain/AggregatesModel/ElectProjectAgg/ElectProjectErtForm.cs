using System;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.EngineerAgg;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public class ElectProjectErtForm
{
    private ElectProjectErtForm(){}


    public ElectProjectErtForm(
        ElectProject electProject,string electrodeAddress, double utmX, double utmY, string constructionDate,
        ElectrodeUsageTypeEnum electrodeUsageTypeEnum,string electrodeUsageTypeOther,
        ElectrodeExecuteTypeEnum electrodeExecuteTypeEnum,string electrodeExecuteTypeOther,
        ElectrodeTypeEnum electrodeTypeEnum,string electrodeTypeOther,
        ElectrodeMaterialTypeEnum electrodeMaterialTypeEnum,string electrodeMaterialTypeOther,
        string electrodeLength, string electrodeDiameter,string otherElectrodeMeasure,
        string des, string ertBrand,string ertTesterBrand, string measurementDate,string measurementHour,
        string temperature, string rainfallAmount, string electrodeResistanceValue,string measurementMethod
        )
    {
        Id = Guid.NewGuid();
        Version = 1;
        ElectProject = electProject;
        ElectrodeAddress = electrodeAddress;
        UtmX = utmX;
        UtmY = utmY;
        ConstructionDate = constructionDate;
        ElectrodeUsageTypeEnum = electrodeUsageTypeEnum;
        ElectrodeUsageTypeOther = electrodeUsageTypeOther;
        ElectrodeExecuteTypeEnum = electrodeExecuteTypeEnum;
        ElectrodeExecuteTypeOther = electrodeExecuteTypeOther;
        ElectrodeTypeEnum = electrodeTypeEnum;
        ElectrodeTypeOther = electrodeTypeOther;
        ElectrodeMaterialTypeEnum = electrodeMaterialTypeEnum;
        ElectrodeMaterialTypeOther = electrodeMaterialTypeOther;
        ElectrodeLength = electrodeLength;
        ElectrodeDiameter = electrodeDiameter;
        OtherElectrodeMeasure = otherElectrodeMeasure;
        Des = des;
        ErtBrand = ertBrand;
        ErtTesterBrand = ertTesterBrand;
        MeasurementDate = measurementDate;
        MeasurementHour = measurementHour;
        Temperature = temperature;
        RainfallAmount = rainfallAmount;
        ElectrodeResistanceValue = electrodeResistanceValue;
        MeasurementMethod = measurementMethod;

        //
        // ReferenceLetterNumber = referenceLetterNumber;
        // SolidTypeEnum = solidTypeEnum;
        // WellDiameter = wellDiameter;
        // WellDepth = wellDepth;
        // RodDiameter = rodDiameter;
        // RodLength = rodLength;
        // RodConnectionConductor = rodConnectionConductor;
        // RodConnectionType = rodConnectionType;
        // PlateSize = plateSize;
        // PlateMaterial = plateMaterial;
        // PlateConnectionConductor = plateConnectionConductor;
        // PlateConnectionType = plateConnectionType;
        // MeshSize = meshSize;
        // MeshMaterial = meshMaterial;
        // MeshConnectionConductor = meshConnectionConductor;
        // MeshConnectionType = meshConnectionType;
        // WireCrossSection = wireCrossSection;
        // WireMaterial = wireMaterial;
        // WireLength = wireLength;
        // WireConnectionConductor = wireConnectionConductor;
        // WireConnectionType = wireConnectionType;
        // BeltCrossSection = beltCrossSection;
        // BeltMaterial = beltMaterial;
        // BeltLength = beltLength;
        // BeltDepth = beltDepth;
        // BeltConnectionType = beltConnectionType;
        // ExpertReport = expertReport;
        // MeasurementErrorPercentage = measurementErrorPercentage;
        // MeasuredResistanceValue = measuredResistanceValue;
        Approved = false;
    }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public ElectProject ElectProject { get; set; }
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




        // پارامتر های قدیمی
        // شماره نامه ارجاعی
        public string ReferenceLetterNumber { get; set; }
        // جنس خاک: رسی،سنگلاخی،شنی،سنگی
        public SolidTypeEnum SolidTypeEnum { get; set; }
        // ارت چاه: قطر-عمق
        public double WellDiameter { get; set; }
        public double WellDepth { get; set; }
        // ارت میله:قطر-طول-هادی اتصال-نوع اتصال
        public double RodDiameter { get; set; }
        public double RodLength { get; set; }
        public string RodConnectionConductor { get; set; }
        public string RodConnectionType { get; set; }
        // ارت صفحه:اندازه-جنس-هادی اتصال-نوع اتصال
        public double PlateSize { get; set; }
        public string PlateMaterial { get; set; }
        public string PlateConnectionConductor { get; set; }
        public string PlateConnectionType { get; set; }
        // ارت مشبک:اندازه-جنس-هادی اتصال-نوع اتصال
        public double MeshSize { get; set; }
        public string MeshMaterial { get; set; }
        public string MeshConnectionConductor { get; set; }
        public string MeshConnectionType { get; set; }
        // ارت گسترده: سیم :مقطع-جنس-طول-هادی اتصال-نوع اتصال
        public double WireCrossSection { get; set; }
        public string WireMaterial { get; set; }
        public double WireLength { get; set; }
        public string WireConnectionConductor { get; set; }
        public string WireConnectionType { get; set; }
        // ارت گسترده: تسمه :مقطع-جنس-طول-هادی اتصال-نوع اتصال
        public double BeltCrossSection { get; set; }
        public string BeltMaterial { get; set; }
        public double BeltLength { get; set; }
        public double BeltDepth { get; set; }
        public string BeltConnectionType { get; set; }
        // مقدار مقاومت اندازه گیری
        public double MeasuredResistanceValue { get; set; }
        // گزارش کارشناسی
        public string ExpertReport { get; set; }
        // درصد خطای اندازه گیری
        public double MeasurementErrorPercentage { get; set; }
        public bool Approved { get; set; }
        public long Amount { get; set; }



        public void Update( 
            string electrodeAddress, double utmX, double utmY, string constructionDate,
            ElectrodeUsageTypeEnum electrodeUsageTypeEnum, string electrodeUsageTypeOther,
            ElectrodeExecuteTypeEnum electrodeExecuteTypeEnum, string electrodeExecuteTypeOther,
            ElectrodeTypeEnum electrodeTypeEnum, string electrodeTypeOther,
            ElectrodeMaterialTypeEnum electrodeMaterialTypeEnum, string electrodeMaterialTypeOther,
            string electrodeLength, string electrodeDiameter, string otherElectrodeMeasure,
            string des, string ertBrand, string ertTesterBrand, string measurementDate, string measurementHour,
            string temperature, string rainfallAmount, string electrodeResistanceValue, string measurementMethod,
            long amount
            )
    {
        ElectrodeAddress = electrodeAddress;
        UtmX = utmX;
        UtmY = utmY;
        ConstructionDate = constructionDate;
        ElectrodeUsageTypeEnum = electrodeUsageTypeEnum;
        ElectrodeUsageTypeOther = electrodeUsageTypeOther;
        ElectrodeExecuteTypeEnum = electrodeExecuteTypeEnum;
        ElectrodeExecuteTypeOther = electrodeExecuteTypeOther;
        ElectrodeTypeEnum = electrodeTypeEnum;
        ElectrodeTypeOther = electrodeTypeOther;
        ElectrodeMaterialTypeEnum = electrodeMaterialTypeEnum;
        ElectrodeMaterialTypeOther = electrodeMaterialTypeOther;
        ElectrodeLength = electrodeLength;
        ElectrodeDiameter = electrodeDiameter;
        OtherElectrodeMeasure = otherElectrodeMeasure;
        Des = des;
        ErtBrand = ertBrand;
        ErtTesterBrand = ertTesterBrand;
        MeasurementDate = measurementDate;
        MeasurementHour = measurementHour;
        Temperature = temperature;
        RainfallAmount = rainfallAmount;
        ElectrodeResistanceValue = electrodeResistanceValue;
        MeasurementMethod = measurementMethod;

        Amount = amount;
    }


}
