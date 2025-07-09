using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Liveness probe - checks if the API process is running.
        /// </summary>
        [HttpGet("live")]
        public IActionResult Liveness()
        {
            return Ok(new { status = "Healthy", time = DateTime.UtcNow });
        }

        /// <summary>
        /// Readiness probe - checks if the API is ready to serve requests (e.g., can connect to DB).
        /// </summary>
        [HttpGet("ready")]
        public IActionResult Readiness([FromServices] App.Database.AppContext db)
        {
            try
            {
                // Simple DB check: try to query something trivial
                db.Database.CanConnect();
                return Ok(new { status = "Ready", time = DateTime.UtcNow });
            }
            catch
            {
                return StatusCode(503, new { status = "Unhealthy", time = DateTime.UtcNow });
            }
        }
    }
}
