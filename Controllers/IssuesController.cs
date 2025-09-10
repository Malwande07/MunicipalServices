using Microsoft.AspNetCore.Mvc;
using MunicipalServices.Data;
using MunicipalServices.Models;
using System.IO;

namespace MunicipalServices.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IssueStore _issueStore;
        private readonly PointsService _pointsService;

        // Constructor injection
        public IssuesController(PointsService pointsService)
        {
            _pointsService = pointsService;
            _issueStore = new IssueStore(); // Can be made injectable later
        }

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

            _issueStore.AddIssue(title, description, location, category, filePath);

            string username = "GuestUser"; // Temporary demo user
            _pointsService.AddPoints(username, 10);

            TempData["Success"] = $"✅ Your issue has been reported! 🎉 You earned 10 points.";
            return RedirectToAction("List");
        }

        // GET: /Issues/List
        public IActionResult List()
        {
            var issues = _issueStore.GetAllIssues();
            ViewBag.Points = _pointsService.GetPoints("GuestUser");
            return View(issues);
        }

        // GET: /Issues/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var issue = _issueStore.GetIssueById(id);
            if (issue == null) return NotFound();
            return View(issue);
        }

        // POST: /Issues/Edit
        [HttpPost]
        public IActionResult Edit(Issue issue)
        {
            _issueStore.UpdateIssue(issue);
            TempData["Success"] = "✏️ Issue updated successfully!";
            return RedirectToAction("List");
        }

        // GET: /Issues/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var issue = _issueStore.GetIssueById(id);
            if (issue == null) return NotFound();
            return View(issue);
        }

        // POST: /Issues/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _issueStore.DeleteIssue(id);
            TempData["Success"] = "🗑️ Issue deleted successfully!";
            return RedirectToAction("List");
        }
    }
}
