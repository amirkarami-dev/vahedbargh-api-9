using Coreapi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.ViewModels
{
    public class ClientExecutorsViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid ClientId { get; set; }
        public OwnershipTypeEnum OwnershipTypeEnum { get; set; }
        public ExecutorTypeEnum ExecutorTypeEnum { get; set; }
        public ExecutorGradTypeEnum ExecutorGradTypeEnum { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public string Tel { get; set; }
        public string NaCode { get; set; }
        public string CellPhone { get; set; }
        public bool License { get; set; }
        public string LicenseNumber { get; set; }
        public int IdSection { get; set; }
        public string Address { get; set; }
        public string MoreInfo { get; set; }
        public string SignatureFileName { get; set; }
        public string LicenseFileName { get; set; }
        public long Balance { get; set; }
        public bool Inactive { get; set; }
        public string UserName { get; set; }
       
    }


}
