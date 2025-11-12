using System;
using System.Collections.Generic;

namespace MunicipalServices.Models
{
    public class ServiceRequest
    {
        public string RequestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string Location { get; set; }
        public int Priority { get; set; } // 1 = Highest, 5 = Lowest
        public string AssignedDepartment { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();
    }

    public enum RequestStatus
    {
        Submitted,
        InProgress,
        OnHold,
        Completed,
        Cancelled,  // ✅ Added this
        Pending
    }
}