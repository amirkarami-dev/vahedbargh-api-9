using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Models.ElectCo
{
    public class ElectCoMapModel
    {
        public string type { get; set; }
        public string id { get; set; }

        public List<MapModelAttr> attributes { get; set; }
    }

    public class MapModelAttr
    {
        public string shamsiDate { get; set; }
        public string address { get; set; }
        public string nationalId { get; set; }
        public string ownerName { get; set; }
        public string buildingArea { get; set; }
        public string counterCapacity { get; set; }
        public string numberOfUnits { get; set; }
        public string consumeType { get; set; }
        public string consumeMax { get; set; }
        public string previousCustomerStatus { get; set; }
        public string previousCustomerDesc { get; set; }
        public string usageType { get; set; }
        public string usageTypeCode { get; set; }
        public string usageTypeDesc { get; set; }
    }

    public class ElectCoMapAttachModel
    {
        public string documentId { get; set; }
        public List<MapAttach> attachments { get; set; }
    }

    public class MapAttach
    {
        public List<MapAttachPro> nationalCardFront { get; set; }
        public List<MapAttachPro> mapFront { get; set; }
        public List<MapAttachPro> mapBack { get; set; }
        public List<MapAttachPro> deed_1 { get; set; }
    }

    public class MapAttachPro
    {
        public string name { get; set; }
        public string hash { get; set; }
        public string mimeType { get; set; }
        public string image { get; set; }
    }
}
