using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Common.Models
{
    public class NotificationModel
    {
        public Guid Id { get; init; }
        public Guid RefId { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public int RefStatus { get; set; }
        public string Message { get; set; }

        public NotificationModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
