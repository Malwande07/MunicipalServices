using MunicipalServices.Models;

namespace MunicipalServices.Data
{
    public class IssueStore
    {
        private static List<Issue> issues = new List<Issue>();
        private static int nextId = 1;

        // ➕ Add issue
        public void AddIssue(string title, string description, string location, string category, string filePath)
        {
            var issue = new Issue
            {
                Id = nextId++,
                Title = string.IsNullOrEmpty(title) ? "Untitled Issue" : title,
                Description = description,
                Location = location,
                Category = category,
                AttachmentPath = filePath,
                DateReported = DateTime.Now
            };
            issues.Add(issue);
        }

        // 📋 Get all issues
        public List<Issue> GetAllIssues()
        {
            return issues;
        }

        // 🔍 Get issue by ID
        public Issue GetIssueById(int id)
        {
            return issues.FirstOrDefault(i => i.Id == id);
        }

        // ✏️ Update issue
        public void UpdateIssue(Issue updatedIssue)
        {
            var existing = GetIssueById(updatedIssue.Id);
            if (existing != null)
            {
                existing.Title = updatedIssue.Title;
                existing.Description = updatedIssue.Description;
                existing.Location = updatedIssue.Location;
                existing.Category = updatedIssue.Category;
                existing.AttachmentPath = updatedIssue.AttachmentPath;
            }
        }

        // 🗑️ Delete issue
        public void DeleteIssue(int id)
        {
            var issue = GetIssueById(id);
            if (issue != null)
            {
                issues.Remove(issue);
            }
        }
    }
}
