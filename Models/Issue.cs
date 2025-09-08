namespace MunicipalServices.Models
{
    public class Issue
    {
        public int Id { get; set; }               // Unique ID
        public string Title { get; set; }         // Short title
        public string Description { get; set; }   // Detailed description
        public string Location { get; set; }      // Where the issue is
        public string Category { get; set; }      // Roads, Utilities, etc.
        public string AttachmentPath { get; set; } // File (image/pdf)
        public DateTime DateReported { get; set; } // Timestamp
    }
}
