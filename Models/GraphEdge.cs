using System;

namespace MunicipalServices.Models
{
    public class GraphEdge
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public int Weight { get; set; }

        public GraphEdge(string source, string destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }
}