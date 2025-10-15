using System;
using System.Collections.Generic;


namespace MunicipalServices.Models
{
    public class EventItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Category { get; set; }

        public DateTime Date { get; set; }
        public int PopularityScore { get; set; } = 0; // For recommendation
        public override string ToString() => $"{Title} ({Start:d})";
    }
}