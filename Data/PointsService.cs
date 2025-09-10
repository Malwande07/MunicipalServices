using MunicipalServices.Models;
using System.Collections.Generic;

namespace MunicipalServices.Data
{
    // Simple PointsService using a Dictionary instead of custom linked list
    public class PointsService
    {
        // Dictionary stores username → points
        private readonly Dictionary<string, int> _pointsDb = new Dictionary<string, int>();

        // Add points to a user
        public void AddPoints(string username, int points)
        {
            if (_pointsDb.ContainsKey(username))
            {
                _pointsDb[username] += points;
            }
            else
            {
                _pointsDb[username] = points;
            }
        }

        // Get points for a user
        public int GetPoints(string username)
        {
            return _pointsDb.ContainsKey(username) ? _pointsDb[username] : 0;
        }
    }
}
