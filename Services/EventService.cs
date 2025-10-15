using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using MunicipalServices.Models;

namespace MunicipalServices.Data
{
    public class EventService
    {
        // Existing collections
        private readonly ConcurrentDictionary<Guid, EventItem> _events = new();
        private readonly ConcurrentDictionary<string, List<string>> _searchHistory = new();
        private readonly ConcurrentBag<EventItem> _upcoming = new();

        // ========================
        // NEW: Sets for Unique Values
        // ========================
        private readonly HashSet<string> _uniqueCategories = new(StringComparer.OrdinalIgnoreCase);
        private readonly SortedSet<DateTime> _uniqueDates = new();

        // ========================
        // NEW: Sorted Dictionary for Date-Organized Events
        // ========================
        private readonly SortedDictionary<DateTime, List<EventItem>> _eventsByDate = new();

        // ========================
        // NEW: Priority Queue for Popularity-Based Ordering
        // ========================
        private readonly PriorityQueue<EventItem, int> _popularityQueue = new();

        // Existing structures
        private readonly object _lastViewedLock = new object();
        private readonly List<Guid> _lastViewed = new();
        private readonly Queue<EventItem> _submissionQueue = new();
        private readonly ConcurrentDictionary<string, LinkedList<(string category, DateTime when)>> _searchRecency = new();

        // ========================
        // Add Event Method (Updated)
        // ========================
        public void AddEvent(EventItem e)
        {
            if (e == null) return;

            _events.TryAdd(e.Id, e);
            _upcoming.Add(e);

            if (!string.IsNullOrWhiteSpace(e.Category))
                _uniqueCategories.Add(e.Category);

            _uniqueDates.Add(e.Start.Date);

            // Add to Sorted Dictionary
            var dateKey = e.Start.Date;
            if (!_eventsByDate.ContainsKey(dateKey))
                _eventsByDate[dateKey] = new List<EventItem>();
            _eventsByDate[dateKey].Add(e);

            // Add to Priority Queue (higher popularity = higher priority)
            _popularityQueue.Enqueue(e, -e.PopularityScore);
        }

        // ========================
        // Get Event by ID
        // ========================
        public EventItem GetEventById(Guid id)
        {
            _events.TryGetValue(id, out var eventItem);
            return eventItem;
        }

        // ========================
        // Get All Events
        // ========================
        public IEnumerable<EventItem> GetAllEvents()
        {
            return _events.Values.OrderBy(e => e.Start);
        }

        // ========================
        // Get All Unique Categories
        // ========================
        public IEnumerable<string> GetCategories()
        {
            return _uniqueCategories.OrderBy(c => c);
        }

        // ========================
        // Get All Unique Dates
        // ========================
        public IEnumerable<DateTime> GetUniqueDates()
        {
            return _uniqueDates;
        }

        // ========================
        // Get Events by Date Range
        // ========================
        public IEnumerable<EventItem> GetEventsByDateRange(DateTime from, DateTime to)
        {
            var results = new List<EventItem>();

            var fromDate = from.Date;
            var toDate = to.Date;

            foreach (var kvp in _eventsByDate)
            {
                if (kvp.Key >= fromDate && kvp.Key <= toDate)
                    results.AddRange(kvp.Value);
                else if (kvp.Key > toDate)
                    break;
            }

            return results;
        }

        // ========================
        // Get Events by Category
        // ========================
        public IEnumerable<EventItem> GetEventsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return _events.Values;

            return _events.Values
                .Where(e => string.Equals(e.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        // ========================
        // Advanced Search
        // ========================
        public IEnumerable<EventItem> Search(string keyword, string category,
            DateTime? from, DateTime? to)
        {
            var results = _events.Values.AsEnumerable();

            // Filter by keyword
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                results = results.Where(e =>
                    (!string.IsNullOrEmpty(e.Title) && e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(e.Description) && e.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category) && _uniqueCategories.Contains(category))
            {
                results = results.Where(e =>
                    string.Equals(e.Category, category, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by date range
            if (from.HasValue && to.HasValue)
            {
                results = results.Where(e =>
                    e.Start.Date >= from.Value.Date && e.Start.Date <= to.Value.Date);
            }
            else if (from.HasValue)
            {
                results = results.Where(e => e.Start.Date >= from.Value.Date);
            }
            else if (to.HasValue)
            {
                results = results.Where(e => e.Start.Date <= to.Value.Date);
            }

            return results.OrderBy(e => e.Start);
        }

        // ========================
        // Get Most Popular Events
        // ========================
        public IEnumerable<EventItem> GetMostPopularEvents(int count = 5)
        {
            return _events.Values
                .OrderByDescending(e => e.PopularityScore)
                .Take(count)
                .ToList();
        }

        // ========================
        // Last Viewed Stack
        // ========================
        public void PushLastViewed(EventItem e)
        {
            if (e == null) return;

            var id = e.Id;
            lock (_lastViewedLock)
            {
                _lastViewed.RemoveAll(g => g == id);
                _lastViewed.Insert(0, id);

                if (_lastViewed.Count > 50)
                    _lastViewed.RemoveRange(50, _lastViewed.Count - 50);
            }
        }

        public IEnumerable<EventItem> GetLastViewed(int max = 5)
        {
            lock (_lastViewedLock)
            {
                return _lastViewed
                    .Take(max)
                    .Select(id => _events.TryGetValue(id, out var e) ? e : null)
                    .Where(e => e != null)
                    .ToList();
            }
        }

        // ========================
        // Submission Queue
        // ========================
        public void EnqueueSubmission(EventItem e)
        {
            if (e == null) return;
            _submissionQueue.Enqueue(e);
            AddEvent(e);
        }

        public EventItem? DequeueSubmission()
        {
            return _submissionQueue.Count > 0 ? _submissionQueue.Dequeue() : null;
        }

        public IEnumerable<EventItem> PeekSubmissions(int max = 10)
        {
            return _submissionQueue.Take(max).ToList();
        }

        // ========================
        // Search Tracking
        // ========================
        public void RecordSearch(string username, string category)
        {
            if (string.IsNullOrEmpty(username))
                username = "Guest";

            var list = _searchHistory.GetOrAdd(username, _ => new List<string>());
            if (!string.IsNullOrWhiteSpace(category))
            {
                lock (list)
                {
                    list.Add(category);
                    if (list.Count > 200)
                        list.RemoveRange(0, list.Count - 200);
                }
            }

            var recency = _searchRecency.GetOrAdd(username, _ => new LinkedList<(string, DateTime)>());
            if (!string.IsNullOrWhiteSpace(category))
            {
                lock (recency)
                {
                    recency.AddFirst((category, DateTime.UtcNow));
                    while (recency.Count > 200)
                        recency.RemoveLast();
                }
            }
        }

        // ========================
        // Recommendation System
        // ========================
        public IEnumerable<EventItem> Recommend(string username, int max = 5)
        {
            username ??= "Guest";

            if (!_searchHistory.TryGetValue(username, out var history) || history.Count == 0)
                return GetMostPopularEvents(max);

            List<string> historySnapshot;
            lock (history)
            {
                historySnapshot = new List<string>(history);
            }

            var freq = historySnapshot
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

            var recencyWeights = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            if (_searchRecency.TryGetValue(username, out var recencyList))
            {
                lock (recencyList)
                {
                    int pos = 0;
                    foreach (var (cat, _) in recencyList)
                    {
                        double w = Math.Max(0.1, 1.0 - (pos * 0.02));
                        recencyWeights[cat] = recencyWeights.GetValueOrDefault(cat) + w;
                        pos++;
                        if (pos > 100) break;
                    }
                }
            }

            var categoryScore = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in freq)
            {
                var cat = kv.Key;
                var f = kv.Value;
                var r = recencyWeights.GetValueOrDefault(cat, 0.0);
                categoryScore[cat] = f * 1.0 + r * 2.0;
            }

            var topCategories = categoryScore
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .Take(3)
                .ToList();

            if (!topCategories.Any())
                return GetMostPopularEvents(max);

            return _events.Values
                .Where(e => topCategories.Contains(e.Category, StringComparer.OrdinalIgnoreCase))
                .OrderByDescending(e => e.PopularityScore)
                .ThenBy(e => e.Start)
                .Take(max)
                .ToList();
        }
    }
}
