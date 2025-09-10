using System;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServices.Models
{
    public class Issue
    {
        public int Id { get; set; } // Unique ID

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        public string AttachmentPath { get; set; } // File (image/pdf)

        public DateTime DateReported { get; set; } = DateTime.Now; // Timestamp
    }
}
