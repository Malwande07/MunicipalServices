using System;
using System.Collections.Generic;

namespace MunicipalServices.Models
{
    public class GraphNode
    {
        public string RequestId { get; set; }
        public ServiceRequest Request { get; set; }
        public List<GraphNode> Neighbors { get; set; }

        public GraphNode(ServiceRequest request)
        {
            RequestId = request.RequestId;
            Request = request;
            Neighbors = new List<GraphNode>();
        }
    }
}