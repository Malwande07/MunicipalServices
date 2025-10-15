using Microsoft.AspNetCore.Mvc;
using MunicipalServices.Data;
using System;
using System.Linq;

namespace MunicipalServices.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Local Events & Announcements";

            var username = User?.Identity?.Name ?? "Guest";

            // Get all events to display
            var allEvents = _eventService.GetMostPopularEvents(20).ToList();

            var recommendations = _eventService.Recommend(username).ToList();
            var lastViewed = _eventService.GetLastViewed(5).ToList();

            ViewBag.Categories = _eventService.GetCategories();
            ViewBag.Recommendations = recommendations;
            ViewBag.LastViewed = lastViewed;

            return View(allEvents);
        }

        [HttpPost]
        public IActionResult Search(string keyword, string category, DateTime? from, DateTime? to)
        {
            ViewData["Title"] = "Search Results";

            var username = User?.Identity?.Name ?? "Guest";

            // Record the search
            _eventService.RecordSearch(username, category);

            // Perform search with all criteria
            var results = _eventService.Search(keyword, category, from, to).ToList();

            // Get recommendations based on updated search history
            var recommendations = _eventService.Recommend(username).ToList();
            var lastViewed = _eventService.GetLastViewed(5).ToList();

            ViewBag.Categories = _eventService.GetCategories();
            ViewBag.Recommendations = recommendations;
            ViewBag.LastViewed = lastViewed;
            ViewBag.SearchCategory = category;

            return View("Index", results);
        }

        public IActionResult ViewEvent(Guid id)
        {
            // Assuming you have a method to get an event by ID
            // For now, this is a placeholder
            // _eventService.PushLastViewed(eventItem);

            return View();
        }
    }
}