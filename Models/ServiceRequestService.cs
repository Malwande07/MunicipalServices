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
            }
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