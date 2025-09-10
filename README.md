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
