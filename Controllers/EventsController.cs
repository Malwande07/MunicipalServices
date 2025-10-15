using Microsoft.AspNetCore.Mvc;
using MunicipalServices.Data;
using MunicipalServices.Models;
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

        public IActionResult Index(string sortBy = "date")
        {
            ViewData["Title"] = "Local Events & Announcements";

            var username = User?.Identity?.Name ?? "Guest";

            // Get all events to display as List
            var allEvents = _eventService.GetAllEvents().ToList();

            // Apply sorting
            allEvents = SortEvents(allEvents, sortBy);

            var recommendations = _eventService.Recommend(username).ToList();
            var lastViewed = _eventService.GetLastViewed(5).ToList();

            ViewBag.Categories = _eventService.GetCategories();
            ViewBag.Recommendations = recommendations;
            ViewBag.LastViewed = lastViewed;
            ViewBag.CurrentSort = sortBy;

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
            // Get the event from service
            var eventItem = _eventService.GetEventById(id);

            if (eventItem == null)
            {
                TempData["Error"] = "Event not found.";
                return RedirectToAction("Index");
            }

            // Track as viewed
            _eventService.PushLastViewed(eventItem);

            return View(eventItem);
        }

        // Helper method for sorting
        private List<EventItem> SortEvents(List<EventItem> events, string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "name" => events.OrderBy(e => e.Title).ToList(),
                "category" => events.OrderBy(e => e.Category).ThenBy(e => e.Start).ToList(),
                "popularity" => events.OrderByDescending(e => e.PopularityScore).ToList(),
                "date" or _ => events.OrderBy(e => e.Start).ToList()
            };
        }
    }
}

/*
 * References and Bibliography
Municipal Services Local Events System

Technical Documentation and Frameworks
ASP.NET Core MVC
Microsoft. (2024). ASP.NET Core MVC overview. Microsoft Learn. https://learn.microsoft.com/en-us/aspnet/core/mvc/overview
Microsoft. (2024). Get started with ASP.NET Core MVC. Microsoft Learn. https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc
Microsoft. (2024). Dependency injection in ASP.NET Core. Microsoft Learn. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
C# Programming Language
Microsoft. (2024). C# documentation. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/
Microsoft. (2024). LINQ (Language Integrated Query). Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
Microsoft. (2024). Pattern matching overview. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching

Data Structures and Algorithms
Collections and Data Structures
Microsoft. (2024). Collections (C#). Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/collections
Microsoft. (2024). System.Collections.Generic Namespace. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic
Microsoft. (2024). System.Collections.Concurrent Namespace. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent
Specific Data Structures
Dictionary and HashTable
Microsoft. (2024). Dictionary<TKey,TValue> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2
Microsoft. (2024). ConcurrentDictionary<TKey,TValue> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2
Cormen, T. H., Leiserson, C. E., Rivest, R. L., & Stein, C. (2022). Introduction to algorithms (4th ed.). MIT Press.
Sorted Dictionary
Microsoft. (2024). SortedDictionary<TKey,TValue> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2
Goodrich, M. T., Tamassia, R., & Goldwasser, M. H. (2014). Data structures and algorithms in Java (6th ed.). Wiley.
HashSet and SortedSet
Microsoft. (2024). HashSet<T> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1
Microsoft. (2024). SortedSet<T> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedset-1
Sedgewick, R., & Wayne, K. (2011). Algorithms (4th ed.). Addison-Wesley Professional.
Stack and Queue
Microsoft. (2024). Stack<T> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1
Microsoft. (2024). Queue<T> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1
Weiss, M. A. (2014). Data structures and algorithm analysis in C++ (4th ed.). Pearson.
Priority Queue
Microsoft. (2024). PriorityQueue<TElement,TPriority> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2
Skiena, S. S. (2020). The algorithm design manual (3rd ed.). Springer.
LinkedList
Microsoft. (2024). LinkedList<T> Class. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1
Karumanchi, N. (2016). Data structures and algorithms made easy: Data structures and algorithmic puzzles (5th ed.). CareerMonk Publications.

Algorithm Design and Optimization
Sorting Algorithms
Knuth, D. E. (1998). The art of computer programming, Volume 3: Sorting and searching (2nd ed.). Addison-Wesley Professional.
Microsoft. (2024). Enumerable.OrderBy Method. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderby
Microsoft. (2024). Enumerable.OrderByDescending Method. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderbydescending
Search Algorithms
Microsoft. (2024). Enumerable.Where Method. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
Bhargava, A. Y. (2016). Grokking algorithms: An illustrated guide for programmers and other curious people. Manning Publications.
Time Complexity and Big O Notation
Cormen, T. H., Leiserson, C. E., Rivest, R. L., & Stein, C. (2022). Introduction to algorithms (4th ed.). MIT Press. [Chapter 3: Growth of Functions]
McDowell, G. L. (2015). Cracking the coding interview: 189 programming questions and solutions (6th ed.). CareerCup.

Recommendation Systems
Collaborative Filtering and Pattern Analysis
Ricci, F., Rokach, L., & Shapira, B. (2015). Recommender systems handbook (2nd ed.). Springer.
Aggarwal, C. C. (2016). Recommender systems: The textbook. Springer.
Frequency and Recency Weighting
Adomavicius, G., & Tuzhilin, A. (2005). Toward the next generation of recommender systems: A survey of the state-of-the-art and possible extensions. IEEE Transactions on Knowledge and Data Engineering, 17(6), 734-749. https://doi.org/10.1109/TKDE.2005.99
Ding, Y., & Li, X. (2005). Time weight collaborative filtering. Proceedings of the 14th ACM International Conference on Information and Knowledge Management, 485-492. https://doi.org/10.1145/1099554.1099689

Concurrent Programming and Thread Safety
Thread-Safe Collections
Microsoft. (2024). Thread-safe collections. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/
Microsoft. (2024). BlockingCollection overview. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview
Lock Statements and Synchronization
Microsoft. (2024). lock statement - ensure exclusive access to a shared resource. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock
Albahari, J., & Albahari, B. (2022). C# 11.0 in a nutshell: The definitive reference. O'Reilly Media.

Web Development and User Interface
Bootstrap Framework
Bootstrap Team. (2024). Bootstrap documentation (Version 5.3). https://getbootstrap.com/docs/5.3/
Otto, M., & Thornton, J. (2024). Bootstrap: The most popular HTML, CSS, and JS library. Bootstrap. https://getbootstrap.com/
Responsive Web Design
Marcotte, E. (2011). Responsive web design. A Book Apart.
Frain, B. (2022). Responsive web design with HTML5 and CSS (4th ed.). Packt Publishing.
HTML5 and CSS3
Mozilla Developer Network. (2024). HTML: HyperText Markup Language. MDN Web Docs. https://developer.mozilla.org/en-US/docs/Web/HTML
Mozilla Developer Network. (2024). CSS: Cascading Style Sheets. MDN Web Docs. https://developer.mozilla.org/en-US/docs/Web/CSS

Software Design Patterns
MVC Pattern
Gamma, E., Helm, R., Johnson, R., & Vlissides, J. (1994). Design patterns: Elements of reusable object-oriented software. Addison-Wesley Professional.
Freeman, E., Robson, E., Bates, B., & Sierra, K. (2020). Head first design patterns: Building extensible and maintainable object-oriented software (2nd ed.). O'Reilly Media.
Repository Pattern
Fowler, M. (2002). Patterns of enterprise application architecture. Addison-Wesley Professional.
Microsoft. (2024). Repository pattern. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design
Dependency Injection
Seemann, M. (2019). Dependency injection principles, practices, and patterns. Manning Publications.
Microsoft. (2024). Dependency injection guidelines. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines

Software Architecture and Best Practices
Clean Code and SOLID Principles
Martin, R. C. (2008). Clean code: A handbook of agile software craftsmanship. Prentice Hall.
Martin, R. C. (2017). Clean architecture: A craftsman's guide to software structure and design. Prentice Hall.
Code Organization
Microsoft. (2024). ASP.NET Core fundamentals. Microsoft Learn. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/
McConnell, S. (2004). Code complete: A practical handbook of software construction (2nd ed.). Microsoft Press.

Performance Optimization
Time and Space Complexity
Skiena, S. S. (2020). The algorithm design manual (3rd ed.). Springer. [Chapter 2: Algorithm Analysis]
Heineman, G. T., Pollice, G., & Selkow, S. (2016). Algorithms in a nutshell: A practical guide (2nd ed.). O'Reilly Media.
Caching and Memory Management
Microsoft. (2024). Memory management and garbage collection in .NET. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/
Richter, J. (2012). CLR via C# (4th ed.). Microsoft Press.

Testing and Debugging
Unit Testing
Microsoft. (2024). Unit testing in .NET. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/core/testing/
Osherove, R. (2013). The art of unit testing: With examples in C# (2nd ed.). Manning Publications.
Debugging Techniques
Microsoft. (2024). Debug your app - Visual Studio. Microsoft Learn. https://learn.microsoft.com/en-us/visualstudio/debugger/
Robbins, J. (2008). Debugging applications for Microsoft .NET and Microsoft Windows. Microsoft Press.

User Experience and Interface Design
Usability Principles
Nielsen, J. (2020). Usability 101: Introduction to usability. Nielsen Norman Group. https://www.nngroup.com/articles/usability-101-introduction-to-usability/
Krug, S. (2014). Don't make me think, revisited: A common sense approach to web usability (3rd ed.). New Riders.
Color Theory and Visual Design
Lidwell, W., Holden, K., & Butler, J. (2010). Universal principles of design (Revised ed.). Rockport Publishers.
Beaird, J., Walker, A., & George, J. (2020). The principles of beautiful web design (4th ed.). SitePoint.

Municipal Services and E-Government
Digital Government Services
United Nations. (2022). E-Government Survey 2022: The future of digital government. United Nations Department of Economic and Social Affairs. https://publicadministration.un.org/egovkb/en-us/Reports/UN-E-Government-Survey-2022
OECD. (2020). Digital Government Index: 2019 results. OECD Public Governance Reviews. https://doi.org/10.1787/4de9f5bb-en
Citizen Engagement Platforms
Mergel, I., Edelmann, N., & Haug, N. (2019). Defining digital transformation: Results from expert interviews. Government Information Quarterly, 36(4). https://doi.org/10.1016/j.giq.2019.06.002
Bertot, J. C., Jaeger, P. T., & Grimes, J. M. (2010). Using ICTs to create a culture of transparency: E-government and social media as openness and anti-corruption tools for societies. Government Information Quarterly, 27(3), 264-271. https://doi.org/10.1016/j.giq.2010.03.001

Version Control and Collaboration
Git and GitHub
Chacon, S., & Straub, B. (2014). Pro Git (2nd ed.). Apress. https://git-scm.com/book/en/v2
GitHub. (2024). GitHub Docs. https://docs.github.com/en

Development Tools
Visual Studio
Microsoft. (2024). Visual Studio documentation. Microsoft Learn. https://learn.microsoft.com/en-us/visualstudio/
NuGet Package Manager
Microsoft. (2024). NuGet documentation. Microsoft Learn. https://learn.microsoft.com/en-us/nuget/

Standards and Conventions
C# Coding Conventions
Microsoft. (2024). C# coding conventions. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
Microsoft. (2024). Naming guidelines. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines
RESTful API Design
Fielding, R. T. (2000). Architectural styles and the design of network-based software architectures [Doctoral dissertation, University of California, Irvine]. https://www.ics.uci.edu/~fielding/pubs/dissertation/top.htm

Academic and Research Papers
Hash Tables and Dictionaries
Knuth, D. E. (1998). The art of computer programming, Volume 3: Sorting and searching (2nd ed.). Addison-Wesley Professional. [Chapter 6: Searching]
Binary Search Trees (Used in SortedDictionary)
Cormen, T. H., Leiserson, C. E., Rivest, R. L., & Stein, C. (2022). Introduction to algorithms (4th ed.). MIT Press. [Chapter 12: Binary Search Trees]
Search Engine Algorithms
Brin, S., & Page, L. (1998). The anatomy of a large-scale hypertextual Web search engine. Computer Networks and ISDN Systems, 30(1-7), 107-117. https://doi.org/10.1016/S0169-7552(98)00110-X

Online Resources and Tutorials
Microsoft Learn
Microsoft. (2024). Microsoft Learn: Training, certification, and documentation. https://learn.microsoft.com/
Stack Overflow
Stack Overflow. (2024). Stack Overflow - Where developers learn, share, & build careers. https://stackoverflow.com/
W3Schools
W3Schools. (2024). W3Schools online web tutorials. https://www.w3schools.com/

Software Development Methodologies
Agile Development
Beck, K., Beedle, M., van Bennekum, A., Cockburn, A., Cunningham, W., Fowler, M., ... & Thomas, D. (2001). Manifesto for Agile Software Development. https://agilemanifesto.org/
Cohn, M. (2010). Succeeding with Agile: Software development using Scrum. Addison-Wesley Professional.

Websites and Online Documentation
.NET Foundation
.NET Foundation. (2024). .NET Foundation. https://dotnetfoundation.org/
ASP.NET Community
ASP.NET Community. (2024). ASP.NET Community Standup. https://dotnet.microsoft.com/en-us/live/aspnet-community-standup
 */