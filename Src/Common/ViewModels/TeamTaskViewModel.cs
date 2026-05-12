using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.ViewModels
{
    public class TeamTaskViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
        public DateTime LastModified { get; set; }
        public IEnumerable<string> Users { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime? EstimatedStartTime { get; set; }
        public DateTime? EstimatedEndTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public bool Archived { get; set; }
        public int AttachmentCount { get; set; }
        public int CommentCount { get; set; }
    }
}
