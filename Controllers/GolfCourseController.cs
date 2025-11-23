using Microsoft.AspNetCore.Mvc;
using SticksAndStonesGCApi.Models;
using SticksAndStonesGCApi.Services;

namespace SticksAndStonesGCApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GolfCourseController : ControllerBase
    {
        private readonly ILogger<GolfCourseController> _logger;
        private readonly IGolfCourseService _repo;

        public GolfCourseController(ILogger<GolfCourseController> logger, IGolfCourseService repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("{courseName}")]
        public GolfCourse GetGolfCourse(string courseName)
        {
            var response = _repo.GetGolfCourseAsync(courseName).Result;

            return response;
        }
    }
}
