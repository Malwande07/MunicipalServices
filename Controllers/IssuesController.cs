using Microsoft.AspNetCore.Mvc;
using MunicipalServices.Data;
using MunicipalServices.Models;

namespace MunicipalServices.Controllers
{
    public class IssuesController : Controller
    {
        private static IssueStore issueStore = new IssueStore();

        // GET: /Issues/Report
        [HttpGet]
        public IActionResult Report()
        {
            return View();
        }

        // POST: /Issues/Report
        [HttpPost]
        public IActionResult Report(string title, string description, string location, string category, IFormFile attachment)
        {
            string filePath = null;

            // ✅ Save uploaded file to wwwroot/uploads
            if (attachment != null && attachment.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                filePath = Path.Combine("/uploads", attachment.FileName);
                using (var stream = new FileStream(Path.Combine(uploads, attachment.FileName), FileMode.Create))
                {
                    attachment.CopyTo(stream);
                }
            }

            // ✅ Store issue in our custom IssueStore (linked list)
            issueStore.AddIssue(title, description, location, category, filePath);

            TempData["Success"] = "✅ Your issue has been reported!";
            return RedirectToAction("List");
        }

        // GET: /Issues/List
        public IActionResult List()
        {
            var issues = issueStore.GetAllIssues();
            return View(issues);
        }

        // GET: /Issues/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var issue = issueStore.GetIssueById(id);
            if (issue == null) return NotFound();
            return View(issue);
        }

        // POST: /Issues/Edit
        [HttpPost]
        public IActionResult Edit(Issue issue)
        {
            issueStore.UpdateIssue(issue);
            TempData["Success"] = "✏️ Issue updated successfully!";
            return RedirectToAction("List");
        }

        // GET: /Issues/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var issue = issueStore.GetIssueById(id);
            if (issue == null) return NotFound();
            return View(issue);
        }

        // POST: /Issues/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            issueStore.DeleteIssue(id);
            TempData["Success"] = "🗑️ Issue deleted successfully!";
            return RedirectToAction("List");
        }
    }
}
