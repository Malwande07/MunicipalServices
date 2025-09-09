using Microsoft.AspNetCore.Mvc;
using MunicipalServices.Data;
using System.Linq; // for ToList()

namespace MunicipalServices.Controllers
{
    public class LeaderboardController : Controller
    {
        private static PointsService pointsService = new PointsService();

        public IActionResult Index()
        {
            var leaderboard = pointsService.GetLeaderboard().ToList(); // ✅ Convert to List
            return View(leaderboard);
        }
    }
}
