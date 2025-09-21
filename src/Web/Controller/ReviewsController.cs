using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    /// <summary>
    /// Get reviews by field
    /// </summary>
    [HttpGet("field/{fieldId}")]
    public IActionResult GetReviewsByField(int fieldId)
    {
        // TODO: Implement
        return Ok(new List<object>());
    }

    /// <summary>
    /// Create a review (requires authentication)
    /// </summary>
    [HttpPost]
    [Authorize]
    public IActionResult CreateReview([FromBody] object request)
    {
        // TODO: Implement
        return Ok(new { message = "Review created" });
    }

    /// <summary>
    /// Update a review (requires authentication)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateReview(int id, [FromBody] object request)
    {
        // TODO: Implement
        return Ok(new { message = "Review updated" });
    }
}