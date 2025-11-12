using System;

namespace MunicipalServices.Models
{
    public class HeapElement
    {
        public ServiceRequest Request { get; set; }
        public int Priority { get; set; }

        public HeapElement(ServiceRequest request, int priority)
        {
            Request = request;
            Priority = priority;
        }
    }
}