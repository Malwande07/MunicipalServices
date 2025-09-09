using MunicipalServices.Models;
using System;

namespace MunicipalServices.Data
{
    // Node for our custom linked list
    public class UserPointsNode
    {
        public UserPoints Data { get; set; }
        public UserPointsNode Next { get; set; }

        public UserPointsNode(UserPoints data)
        {
            Data = data;
            Next = null;
        }
    }

    // Custom linked list for UserPoints
    public class UserPointsList
    {
        private UserPointsNode head;

        public UserPointsList()
        {
            head = null;
        }

        // Add a new user
        public void Add(UserPoints user)
        {
            if (head == null)
            {
                head = new UserPointsNode(user);
            }
            else
            {
                var current = head;
                while (current.Next != null)
                    current = current.Next;
                current.Next = new UserPointsNode(user);
            }
        }

        // Find a user by username
        public UserPoints Find(string username)
        {
            var current = head;
            while (current != null)
            {
                if (current.Data.Username == username)
                    return current.Data;
                current = current.Next;
            }
            return null;
        }

        // Get all users as an array for leaderboard sorting
        public UserPoints[] ToArray()
        {
            int count = 0;
            var current = head;
            while (current != null)
            {
                count++;
                current = current.Next;
            }

            UserPoints[] array = new UserPoints[count];
            current = head;
            int i = 0;
            while (current != null)
            {
                array[i++] = current.Data;
                current = current.Next;
            }
            return array;
        }
    }

    // PointsService using custom linked list
    public class PointsService
    {
        private static UserPointsList _pointsDb = new UserPointsList();

        // Get or create a user
        public UserPoints GetOrCreateUser(string username)
        {
            var user = _pointsDb.Find(username);
            if (user == null)
            {
                user = new UserPoints { Username = username, Points = 0 };
                _pointsDb.Add(user);
            }
            return user;
        }

        // Add points to a user
        public void AddPoints(string username, int points)
        {
            var user = GetOrCreateUser(username);
            user.Points += points;
        }

        // Get points for a user
        public int GetPoints(string username)
        {
            return GetOrCreateUser(username).Points;
        }

        // Get leaderboard sorted descending by points
        public UserPoints[] GetLeaderboard()
        {
            var users = _pointsDb.ToArray();

            // Simple bubble sort (custom, no LINQ)
            for (int i = 0; i < users.Length - 1; i++)
            {
                for (int j = 0; j < users.Length - i - 1; j++)
                {
                    if (users[j].Points < users[j + 1].Points)
                    {
                        var temp = users[j];
                        users[j] = users[j + 1];
                        users[j + 1] = temp;
                    }
                }
            }

            return users;
        }
    }
}
