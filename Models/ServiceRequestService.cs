using MunicipalServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServices.Data
{
    public class ServiceRequestService
    {
        // In-memory storage for service requests
        private List<ServiceRequest> _requests;
        private Dictionary<string, List<string>> _dependencyGraph;

        public ServiceRequestService()
        {
            _requests = new List<ServiceRequest>();
            _dependencyGraph = new Dictionary<string, List<string>>();
        }

        // ========================
        // Basic CRUD Operations
        // ========================
        public List<ServiceRequest> GetAllRequests()
        {
            return _requests.ToList();
        }

        public ServiceRequest GetRequestById(string id)
        {
            return _requests.FirstOrDefault(r => r.RequestId.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public void AddRequest(ServiceRequest request)
        {
            _requests.Add(request);

            // Build dependency graph from the request's Dependencies property
            if (request.Dependencies != null && request.Dependencies.Any())
            {
                if (!_dependencyGraph.ContainsKey(request.RequestId))
                {
                    _dependencyGraph[request.RequestId] = new List<string>();
                }

                foreach (var dependency in request.Dependencies)
                {
                    if (!_dependencyGraph[request.RequestId].Contains(dependency))
                    {
                        _dependencyGraph[request.RequestId].Add(dependency);
                    }
                }
            }
        }

        public void UpdateRequestStatus(string requestId, RequestStatus newStatus)
        {
            var request = GetRequestById(requestId);
            if (request != null)
            {
                request.Status = newStatus;

                // Update DateCompleted if status is Completed
                if (newStatus == RequestStatus.Completed && request.DateCompleted == null)
                {
                    request.DateCompleted = DateTime.Now;
                }

                // Auto-update dependent requests
                if (newStatus == RequestStatus.Completed)
                {
                    AutoResolveOrUnblockDependentRequests(requestId);
                }
            }
        }

        // Automatically update dependent requests
        private void AutoResolveOrUnblockDependentRequests(string completedRequestId)
        {
            // Find all requests that depend on the completed request
            var dependentRequests = _requests.Where(r =>
                r.Dependencies != null &&
                r.Dependencies.Contains(completedRequestId)
            ).ToList();

            foreach (var dependentRequest in dependentRequests)
            {
                // Check if ALL dependencies are now completed
                bool allDependenciesCompleted = dependentRequest.Dependencies.All(depId =>
                {
                    var dep = GetRequestById(depId);
                    return dep != null && dep.Status == RequestStatus.Completed;
                });

                // If all dependencies completed, update status
                if (allDependenciesCompleted)
                {
                    if (dependentRequest.Status == RequestStatus.OnHold)
                    {
                        // Move from OnHold to Submitted (ready to work)
                        dependentRequest.Status = RequestStatus.Submitted;
                    }
                    else if (dependentRequest.Status == RequestStatus.Submitted)
                    {
                        // If it was a "duplicate report", mark as completed
                        if (IsDuplicateReport(dependentRequest))
                        {
                            dependentRequest.Status = RequestStatus.Completed;
                            dependentRequest.DateCompleted = DateTime.Now;
                        }
                    }
                }
            }
        }

        // Determine if request is a duplicate report
        private bool IsDuplicateReport(ServiceRequest request)
        {
            // Logic: If request has same category and location as its dependency
            if (request.Dependencies == null || !request.Dependencies.Any())
                return false;

            var mainDependency = GetRequestById(request.Dependencies.First());
            if (mainDependency == null)
                return false;

            // Check if it's a "no water" report dependent on "water main break"
            return request.Category == mainDependency.Category &&
                   request.Location == mainDependency.Location;
        }

        // ========================
        // BST Operations (Search by ID)
        // ========================
        public ServiceRequest SearchBST(string id)
        {
            return _requests.FirstOrDefault(r => r.RequestId.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public List<ServiceRequest> InOrderTraversalBST()
        {
            return _requests.OrderBy(r => r.RequestId).ToList();
        }

        // ========================
        // Priority-Based Operations
        // ========================
        public List<ServiceRequest> GetRequestsByPriority()
        {
            return _requests.OrderBy(r => r.Priority).ThenBy(r => r.DateSubmitted).ToList();
        }

        public ServiceRequest GetHighestPriorityRequest()
        {
            return _requests.OrderBy(r => r.Priority).FirstOrDefault();
        }

        public List<ServiceRequest> GetAllPriorityOrdered()
        {
            return GetRequestsByPriority();
        }

        // ========================
        // Date-Based Operations
        // ========================
        public List<ServiceRequest> GetRequestsByDate()
        {
            return _requests.OrderBy(r => r.DateSubmitted).ToList();
        }

        // ========================
        // Graph Operations (Dependencies)
        // ========================
        public List<string> GetDependencies(string requestId)
        {
            return _dependencyGraph.ContainsKey(requestId)
                ? _dependencyGraph[requestId]
                : new List<string>();
        }

        public List<string> BFSTraversal(string startId)
        {
            var visited = new HashSet<string>();
            var queue = new Queue<string>();
            var result = new List<string>();

            queue.Enqueue(startId);
            visited.Add(startId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current);

                if (_dependencyGraph.ContainsKey(current))
                {
                    foreach (var neighbor in _dependencyGraph[current])
                    {
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }

            return result;
        }

        public List<string> DFSTraversal(string startId)
        {
            var visited = new HashSet<string>();
            var result = new List<string>();
            DFSHelper(startId, visited, result);
            return result;
        }

        private void DFSHelper(string nodeId, HashSet<string> visited, List<string> result)
        {
            visited.Add(nodeId);
            result.Add(nodeId);

            if (_dependencyGraph.ContainsKey(nodeId))
            {
                foreach (var neighbor in _dependencyGraph[nodeId])
                {
                    if (!visited.Contains(neighbor))
                    {
                        DFSHelper(neighbor, visited, result);
                    }
                }
            }
        }

        public List<(string From, string To, int Weight)> GetMinimumSpanningTree()
        {
            // Simple MST implementation - returns all edges
            var edges = new List<(string From, string To, int Weight)>();

            foreach (var kvp in _dependencyGraph)
            {
                foreach (var dependency in kvp.Value)
                {
                    edges.Add((kvp.Key, dependency, 1)); // Weight = 1 for simplicity
                }
            }

            return edges;
        }

        // ========================
        // Add Dependency
        // ========================
        public void AddDependency(string fromId, string toId)
        {
            if (!_dependencyGraph.ContainsKey(fromId))
            {
                _dependencyGraph[fromId] = new List<string>();
            }

            if (!_dependencyGraph[fromId].Contains(toId))
            {
                _dependencyGraph[fromId].Add(toId);
            }

            // Also update the request's Dependencies list
            var request = GetRequestById(fromId);
            if (request != null && !request.Dependencies.Contains(toId))
            {
                request.Dependencies.Add(toId);
            }
        }
    }
}