using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.ViewModels
{
    public class ClientTeamViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ColorCode { get; set; }
        public DateTime LastModified { get; set; }
        public IEnumerable<ClientTeamUserViewModel> Users { get; set; }
        public Guid? TemplateId { get; set; }
        public bool HaveTask { get; set; }
    }

    public class ClientTeamUserViewModel
    {
        public string UserId { get; set; }
        public bool IsManager { get; set; }
    }
}
