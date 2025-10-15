Municipal Services Reporting App
📖 Introduction

The Municipal Services Reporting App is a lightweight ASP.NET Core MVC application designed to help residents report service delivery issues such as potholes, water leaks, or electricity faults.

It empowers citizens to contribute to their communities while municipalities benefit from structured issue reporting and improved communication.

🎯 Features
📝 Report Issues

Submit issues like potholes, broken lights, and water leaks.

Option to attach photos for clarity.

⭐ Points System

Users earn points for every issue they report.

Points are shown in the navigation bar as a recognition badge.

📋 View Issues

See a list of all reported issues.

Ensures transparency and community awareness.

🔒 Future Features (Coming Soon)

Local Events updates.

Track service request progress.

Community contribution badges.

🏆 Gamification & Micro-Rewards
🎮 Why Gamification?

Gamification introduces game-like elements (points, progress, recognition) to make civic participation engaging. In South Africa, where trust in municipalities can be limited, micro-rewards encourage repeat use without monetary incentives.

⚙️ Core Mechanics Implemented

Contribution Points: Residents earn ⭐ points for every reported issue.

Acknowledgements (Micro-feedback): After reporting, users see an immediate confirmation message (✅ “Thank you for helping improve your community”).

Visible Recognition: A points badge is displayed in the navigation bar so users can track their progress.

Non-monetary Rewards: Recognition through points and public reports instead of cash incentives.

🛠️ Planned Extensions

Badges & Tiers (e.g., First Report, Neighborhood Helper).

Progress Bars showing neighborhood goals (e.g., 20 potholes fixed this month).

Community Visibility: Optional contributor highlights while protecting privacy.

✅ Benefits

Encourages repeat engagement.

Builds trust with municipalities.

Creates community spirit by showing progress.

Inclusive — no costs or connectivity barriers.

🛠️ Tech Stack

Frontend: Razor Views + Bootstrap 5

Backend: ASP.NET Core MVC

Database: SQLite (lightweight, file-based)

Custom Data Structures: Linked List for managing user points

🚀 Getting Started
📦 Prerequisites

.NET 6 SDK or later

Visual Studio 2022 (or Rider / VS Code with C# extension)

▶️ Run the App

Clone this repository

git clone https://github.com/your-repo/municipal-services.git
cd municipal-services


Build the project

dotnet build


Run locally

dotnet run


Open in your browser:

https://localhost:5001



Part 2 
Municipal Services - Local Events System
A web application for managing and discovering local municipal events and announcements.
________________________________________
What This App Does
•	View Events: Browse upcoming local events and announcements
•	Search Events: Find events by keyword, category, or date
•	Sort Events: Order events by date, name, category, or popularity
•	Get Recommendations: See personalized event suggestions based on your searches
•	Track History: View your recently viewed events
________________________________________
 How to Run
Prerequisites
•	Visual Studio 2022
•	.NET 6.0 or higher
Steps
1.	Open the project in Visual Studio
2.	Press F5 or click the green "Run" button
3.	Your browser will open automatically
4.	Navigate to Local Events from the main menu
________________________________________
 Main Features
1. View All Events
•	Go to "Local Events" page
•	See all upcoming events in cards
•	Each card shows: title, description, category, and date
2. Search Events
Use the search form at the top:
•	Keyword: Search by event name or description
•	Category: Filter by type (Community, Safety, Health, etc.)
•	From Date: Set start date
•	To Date: Set end date
•	Click Search button
3. Sort Events
Use the dropdown at top-right:
•	Sort by Date (default)
•	Sort by Name (A-Z)
•	Sort by Category
•	Sort by Popularity
4. View Event Details
•	Click "View Details" on any event card
•	See complete event information
•	Event is saved to your "Recently Viewed" list
5. Recently Viewed
•	Shows your last 5 viewed events
•	Automatically updated when you view events
•	Located below the main event list
6. Recommendations
•	Search for events 2-3 times
•	Scroll to "Recommended for You" section
•	See personalized suggestions based on your interests
________________________________________
🔧 Technical Information
Data Structures Used
•	Dictionary: Fast event storage (O(1) lookup)
•	Sorted Dictionary: Date-organized events for range queries
•	HashSet: Unique categories (no duplicates)
•	Stack: Recently viewed events (Last In, First Out)
•	Queue: Event submissions (First In, First Out)
•	Priority Queue: Popularity-based ordering
Technologies
•	ASP.NET Core MVC
•	C# 10
•	Bootstrap 5
•	HTML5 / CSS3
•	LINQ
________________________________________

🎨 Sample Data
The app comes with 15 pre-loaded events:
•	Community Events: Clean-up days, festivals
•	Safety Events: Fire safety, water safety
•	Health Events: Free screenings, wellness programs
•	Infrastructure: Road maintenance updates
•	Utilities: Electricity, water notices
•	Education: Digital literacy, youth programs
________________________________________
📖 How to Use
Basic Usage
1.	Start the app → Opens in browser
2.	Click "View Events" → See all events
3.	Browse event cards → Find interesting events
4.	Click "View Details" → See full information
Search Example
1.	Select Category: "Community"
2.	Set From Date: Today
3.	Set To Date: 7 days from now
4.	Click Search
5.	See only Community events this week
Get Recommendations
1.	Search for "Community" events (2-3 times)
2.	Search for "Safety" events (1-2 times)
3.	Scroll to "Recommended for You"
4.	See personalized suggestions
________________________________________
✅ Requirements Met
•	✅ Main Menu with 3 options
•	✅ Events display with professional layout
•	✅ Search by keyword, category, and date
•	✅ Sort by date, name, category, popularity
•	✅ Advanced data structures (Dictionaries, Sets, Stack, Queue)
•	✅ Recommendation system (frequency + recency)
•	✅ Recently viewed history
•	✅ Responsive design
________________________________________

