using System;

namespace MunicipalServices.Models
{
    public enum NodeColor
    {
        Red,
        Black
    }

    public class RBNode
    {
        public ServiceRequest Data { get; set; }
        public RBNode Left { get; set; }
        public RBNode Right { get; set; }
        public RBNode Parent { get; set; }
        public NodeColor Color { get; set; }

        public RBNode(ServiceRequest data)
        {
            Data = data;
            Left = null;
            Right = null;
            Parent = null;
            Color = NodeColor.Red;
        }
    }
}