using System;

namespace MunicipalServices.Models
{
    public class BSTNode
    {
        public ServiceRequest Data { get; set; }
        public BSTNode Left { get; set; }
        public BSTNode Right { get; set; }

        public BSTNode(ServiceRequest data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }
}