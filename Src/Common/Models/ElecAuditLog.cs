using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Common.Models;

public class ElecAuditLog
{
    private ElecAuditLog() { }


    public ElecAuditLog(string userId, string userFullName, string solarTimeStamp, string ip, string entityName,long fileNumber, string entityId, EntityState state, DateTime timeStamp)
    {
        UserId = userId;
        UserFullName = userFullName;
        SolarTimeStamp = solarTimeStamp;
        Ip = ip;
        EntityName = entityName;
        FileNumber = fileNumber;
        EntityId = entityId;
        State = state;
        TimeStamp = timeStamp;
    }
        public long Id { get; set; }              // PK
        public string UserId { get; set; }        // The user performing the change
        public string UserFullName { get; set; }
        public string SolarTimeStamp { get; set; }
        public string Ip { get; set; }
        public string EntityName { get; set; }    // e.g. "GasProject", "Executor", etc.
        public string EntityId { get; set; }      // The primary key (string form)
        public EntityState State { get; set; }    // EF Core EntityState: Added, Modified, Deleted
        public DateTime TimeStamp { get; set; }
        public long FileNumber { get; set; }

        // We will store this as JSON. EF 7+ can do an OwnsMany with .ToJson().
        // Alternatively, you can store as a string column, but JSON is nice if your database supports it.
        public List<EntityChangeValueObject> Changes { get; set; } = new();


        public class EntityChangeValueObject
        {
            public string PropertyName { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }


}
