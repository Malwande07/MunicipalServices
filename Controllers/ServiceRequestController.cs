using Microsoft.AspNetCore.Mvc;
using MunicipalServices.Data;
using MunicipalServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServices.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ServiceRequestService _service;

        public ServiceRequestController(ServiceRequestService service)
        {
            _service = service;
        }

        // ========================
        // Main Status Page
        // ========================
        public IActionResult Status(string sortBy = "id", string searchId = null)
        {
            ViewData["Title"] = "Service Request Status";

            List<ServiceRequest> requests;

            // Search by ID if provided
            if (!string.IsNullOrWhiteSpace(searchId))
            {
                var request = _service.SearchBST(searchId);
                requests = request != null ? new List<ServiceRequest> { request } : new List<ServiceRequest>();
                ViewBag.SearchPerformed = true;
                ViewBag.SearchId = searchId;
            }
            else
            {
                // Get requests based on sort option
                requests = sortBy?.ToLower() switch
                {
                    "priority" => _service.GetRequestsByPriority(),
                    "date" => _service.GetRequestsByDate(),
                    "heap" => _service.GetAllPriorityOrdered(),
                    _ => _service.InOrderTraversalBST() // Default: BST (by ID)
                };
            }

            ViewBag.CurrentSort = sortBy;
            ViewBag.TotalRequests = _service.GetAllRequests().Count;
            ViewBag.HighestPriority = _service.GetHighestPriorityRequest();

            return View(requests);
        }

        // ========================
        // View Single Request Details
        // ========================
        public IActionResult Details(string id)
        {
            var request = _service.GetRequestById(id);

            if (request == null)
            {
                TempData["Error"] = "Service request not found.";
                return RedirectToAction("Status");
            }

            // Get dependencies using graph
            var dependencies = _service.GetDependencies(id);
            ViewBag.Dependencies = dependencies;
            ViewBag.BFSTraversal = _service.BFSTraversal(id);
            ViewBag.DFSTraversal = _service.DFSTraversal(id);

            // Get full ServiceRequest objects for each dependency
            var dependencyRequests = new List<ServiceRequest>();
            if (dependencies != null && dependencies.Any())
            {
                foreach (var depId in dependencies)
                {
                    var depRequest = _service.GetRequestById(depId);
                    if (depRequest != null)
                    {
                        dependencyRequests.Add(depRequest);
                    }
                }
            }
            ViewBag.DependencyRequests = dependencyRequests;

            // Find all requests that depend on THIS request
            var allRequests = _service.GetAllRequests();
            var dependentRequests = new List<ServiceRequest>();

            foreach (var req in allRequests)
            {
                if (req.RequestId != id && req.Dependencies != null && req.Dependencies.Contains(id))
                {
                    dependentRequests.Add(req);
                }
            }
            ViewBag.DependentRequests = dependentRequests;

            return View(request);
        }

        // ========================
        // View Dependency Graph
        // ========================
        public IActionResult DependencyGraph()
        {
            ViewData["Title"] = "Request Dependencies";

            var allRequests = _service.GetAllRequests();
            var mst = _service.GetMinimumSpanningTree();

            ViewBag.MinimumSpanningTree = mst;
            ViewBag.TotalRequests = allRequests.Count;

            return View(allRequests);
        }

        // ========================
        // Update Request Status
        // ========================
        [HttpPost]
        public IActionResult UpdateStatus(string requestId, RequestStatus newStatus)
        {
            _service.UpdateRequestStatus(requestId, newStatus);
            TempData["Success"] = $"Request {requestId} updated to {newStatus}";
            return RedirectToAction("Details", new { id = requestId });
        }

        // ========================
        // Priority Queue View
        // ========================
        public IActionResult PriorityQueue()
        {
            ViewData["Title"] = "Priority Queue";

            var priorityOrdered = _service.GetAllPriorityOrdered();
            var highestPriority = _service.GetHighestPriorityRequest();

            ViewBag.HighestPriority = highestPriority;

            return View(priorityOrdered);
        }

        // ========================
        // Tree Visualizations
        // ========================
        public IActionResult TreeView(string treeType = "bst")
        {
            ViewData["Title"] = $"{treeType.ToUpper()} Visualization";

            List<ServiceRequest> requests = treeType?.ToLower() switch
            {
                "avl" => _service.GetRequestsByPriority(),
                "rb" => _service.GetRequestsByDate(),
                _ => _service.InOrderTraversalBST()
            };

            ViewBag.TreeType = treeType.ToUpper();
            ViewBag.Description = treeType?.ToLower() switch
            {
                "bst" => "Binary Search Tree - Ordered by Request ID",
                "avl" => "AVL Tree - Self-balancing, Ordered by Priority",
                "rb" => "Red-Black Tree - Self-balancing, Ordered by Date",
                _ => "Tree Visualization"
            };

            return View(requests);
        }
    }
}