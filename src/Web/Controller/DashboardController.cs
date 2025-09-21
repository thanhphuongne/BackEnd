using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    /// <summary>
    /// Get user dashboard (requires authentication)
    /// </summary>
    [HttpGet]
    [Authorize]
    public IActionResult GetUserDashboard()
    {
        // TODO: Implement
        return Ok(new { message = "Dashboard data" });
    }

    /// <summary>
    /// Get user stats (requires authentication)
    /// </summary>
    [HttpGet("stats")]
    [Authorize]
    public IActionResult GetUserStats()
    {
        // TODO: Implement
        return Ok(new { message = "User stats" });
    }
}