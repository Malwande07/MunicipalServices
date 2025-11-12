using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MunicipalServices.Data;
using MunicipalServices.Models;
using System;
using System.Collections.Generic;

namespace MunicipalServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ✅ Register PointsService as a singleton for global access
            builder.Services.AddSingleton<PointsService>();

            // ✅ Register EventService as a singleton for global access
            builder.Services.AddSingleton<EventService>();

            // ✅ Register ServiceRequestService as a singleton for global access
            builder.Services.AddSingleton<ServiceRequestService>();

            var app = builder.Build();

            // ✅ SEED SAMPLE EVENTS FOR DEMO
            SeedSampleEvents(app);

            // ✅ SEED SAMPLE SERVICE REQUESTS FOR DEMO
            SeedSampleServiceRequests(app);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void SeedSampleEvents(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var eventService = scope.ServiceProvider.GetRequiredService<EventService>();

                // Community Events
                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Community Clean-up Day",
                    Description = "Join us in cleaning up the local park and making our community greener. Volunteers will receive refreshments and community service certificates.",
                    Category = "Community",
                    Start = DateTime.Now.AddDays(5),
                    PopularityScore = 85
                });

                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Local Festival 2025",
                    Description = "Annual municipal festival with music, food stalls, entertainment, and family activities. All ages welcome!",
                    Category = "Community",
                    Start = DateTime.Now.AddDays(10),
                    PopularityScore = 95
                });

                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Environmental Conservation Drive",
                    Description = "Help plant trees and restore natural habitats in our municipal parks. Tools and saplings provided.",
                    Category = "Community",
                    Start = DateTime.Now.AddDays(8),
                    PopularityScore = 78
                });

                // Safety Events
                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Water Safety Workshop",
                    Description = "Learn essential water safety tips for the entire family. Includes CPR training and drowning prevention techniques.",
                    Category = "Safety",
                    Start = DateTime.Now.AddDays(3),
                    PopularityScore = 72
                });

                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Fire Safety Awareness Campaign",
                    Description = "Learn fire prevention, evacuation procedures, and proper use of fire extinguishers. Free smoke detectors available.",
                    Category = "Safety",
                    Start = DateTime.Now.AddDays(6),
                    PopularityScore = 68
                });

                // Infrastructure Events
                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Road Maintenance Update",
                    Description = "Information session about upcoming road repairs in District 5. Learn about traffic diversions and project timeline.",
                    Category = "Infrastructure",
                    Start = DateTime.Now.AddDays(7),
                    PopularityScore = 60
                });

                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "New Bridge Construction Town Hall",
                    Description = "Public meeting to discuss the new bridge project over River Road. Q&A session with engineers and planners.",
                    Category = "Infrastructure",
                    Start = DateTime.Now.AddDays(12),
                    PopularityScore = 55
                });

                // Utilities Events
                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Electricity Maintenance Notice",
                    Description = "Scheduled maintenance work on power lines in Zones A and B. Expected 4-hour outage between 9 AM and 1 PM.",
                    Category = "Utilities",
                    Start = DateTime.Now.AddDays(2),
                    PopularityScore = 50
                });

                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Water Conservation Workshop",
                    Description = "Learn practical tips for reducing water consumption and saving on utility bills. Free water-saving devices provided.",
                    Category = "Utilities",
                    Start = DateTime.Now.AddDays(9),
                    PopularityScore = 64
                });

                // Health Events
                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Free Health Screening",
                    Description = "Free blood pressure, diabetes, and cholesterol screening for all residents. First come, first served.",
                    Category = "Health",
                    Start = DateTime.Now.AddDays(4),
                    PopularityScore = 88
                });

                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Mental Health Awareness Seminar",
                    Description = "Join mental health professionals to discuss stress management, anxiety, and available support services.",
                    Category = "Health",
                    Start = DateTime.Now.AddDays(11),
                    PopularityScore = 75
                });

                // Education Events
                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Digital Literacy Classes",
                    Description = "Free computer and internet skills training for seniors. Learn email, online banking, and social media basics.",
                    Category = "Education",
                    Start = DateTime.Now.AddDays(1),
                    PopularityScore = 70
                });

                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Youth Leadership Program Launch",
                    Description = "Empowering young people with leadership skills, civic engagement, and community service opportunities.",
                    Category = "Education",
                    Start = DateTime.Now.AddDays(15),
                    PopularityScore = 82
                });

                // Additional varied events
                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Waste Management Information Session",
                    Description = "Learn about new recycling initiatives, composting programs, and proper waste disposal methods.",
                    Category = "Community",
                    Start = DateTime.Now.AddDays(14),
                    PopularityScore = 58
                });

                eventService.AddEvent(new EventItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Neighborhood Watch Meeting",
                    Description = "Monthly meeting to discuss community safety concerns and crime prevention strategies.",
                    Category = "Safety",
                    Start = DateTime.Now.AddDays(13),
                    PopularityScore = 62
                });

                Console.WriteLine("✅ Successfully seeded 15 sample events!");
            }
        }

        private static void SeedSampleServiceRequests(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ServiceRequestService>();

                // Request 1 - High Priority
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR001",
                    Title = "Water Main Break - Emergency",
                    Description = "Major water main break causing flooding on Main Street",
                    Category = "Utilities",
                    Status = RequestStatus.InProgress,
                    DateSubmitted = DateTime.Now.AddDays(-5),
                    Location = "Main Street & 5th Ave",
                    Priority = 1,
                    AssignedDepartment = "Water & Sanitation",
                    Dependencies = new List<string>()
                });

                // Request 2 - Depends on SR001
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR002",
                    Title = "Road Repair - Main Street",
                    Description = "Repair road damage caused by water main break",
                    Category = "Infrastructure",
                    Status = RequestStatus.Submitted,
                    DateSubmitted = DateTime.Now.AddDays(-4),
                    Location = "Main Street & 5th Ave",
                    Priority = 2,
                    AssignedDepartment = "Public Works",
                    Dependencies = new List<string> { "SR001" }
                });

                // Request 3 - High Priority
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR003",
                    Title = "Power Outage - Residential Area",
                    Description = "Multiple homes without power in District 5",
                    Category = "Utilities",
                    Status = RequestStatus.InProgress,
                    DateSubmitted = DateTime.Now.AddDays(-3),
                    Location = "District 5",
                    Priority = 1,
                    AssignedDepartment = "Electricity",
                    Dependencies = new List<string>()
                });

                // Request 4
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR004",
                    Title = "Pothole Repair Request",
                    Description = "Large pothole causing traffic hazard",
                    Category = "Infrastructure",
                    Status = RequestStatus.Submitted,
                    DateSubmitted = DateTime.Now.AddDays(-10),
                    Location = "Elm Street",
                    Priority = 2,
                    AssignedDepartment = "Public Works",
                    Dependencies = new List<string>()
                });

                // Request 5
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR005",
                    Title = "Streetlight Malfunction",
                    Description = "Several streetlights not working",
                    Category = "Utilities",
                    Status = RequestStatus.Completed,
                    DateSubmitted = DateTime.Now.AddDays(-15),
                    DateCompleted = DateTime.Now.AddDays(-2),
                    Location = "Oak Avenue",
                    Priority = 3,
                    AssignedDepartment = "Electricity",
                    Dependencies = new List<string>()
                });

                // Request 6 - Depends on SR003
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR006",
                    Title = "Traffic Light Repair",
                    Description = "Traffic lights offline due to power outage",
                    Category = "Safety",
                    Status = RequestStatus.OnHold,
                    DateSubmitted = DateTime.Now.AddDays(-2),
                    Location = "District 5 Intersection",
                    Priority = 1,
                    AssignedDepartment = "Traffic Management",
                    Dependencies = new List<string> { "SR003" }
                });

                // Request 7
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR007",
                    Title = "Garbage Collection Missed",
                    Description = "Scheduled collection not completed",
                    Category = "Sanitation",
                    Status = RequestStatus.InProgress,
                    DateSubmitted = DateTime.Now.AddDays(-1),
                    Location = "Pine Street",
                    Priority = 2,
                    AssignedDepartment = "Waste Management",
                    Dependencies = new List<string>()
                });

                // Request 8
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR008",
                    Title = "Tree Removal - Storm Damage",
                    Description = "Fallen tree blocking road",
                    Category = "Parks",
                    Status = RequestStatus.Completed,
                    DateSubmitted = DateTime.Now.AddDays(-7),
                    DateCompleted = DateTime.Now.AddDays(-5),
                    Location = "Park Avenue",
                    Priority = 1,
                    AssignedDepartment = "Parks & Recreation",
                    Dependencies = new List<string>()
                });

                // Request 9
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR009",
                    Title = "Sidewalk Crack Repair",
                    Description = "Dangerous cracks in sidewalk",
                    Category = "Infrastructure",
                    Status = RequestStatus.Submitted,
                    DateSubmitted = DateTime.Now.AddDays(-6),
                    Location = "Market Street",
                    Priority = 3,
                    AssignedDepartment = "Public Works",
                    Dependencies = new List<string>()
                });

                // Request 10 - Depends on SR001 and SR002
                service.AddRequest(new ServiceRequest
                {
                    RequestId = "SR010",
                    Title = "Water Quality Testing",
                    Description = "Test water quality after main break repair",
                    Category = "Utilities",
                    Status = RequestStatus.Submitted,
                    DateSubmitted = DateTime.Now.AddDays(-4),
                    Location = "Main Street Area",
                    Priority = 2,
                    AssignedDepartment = "Water & Sanitation",
                    Dependencies = new List<string> { "SR001", "SR002" }
                });

                Console.WriteLine("✅ Successfully seeded 10 sample service requests!");
                Console.WriteLine("📊 Data structures initialized: BST, AVL, RB-Tree, Heap, Graph");
            }
        }
    }
}