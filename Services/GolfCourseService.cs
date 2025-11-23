using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SticksAndStonesGCApi.Data;
using SticksAndStonesGCApi.Models;

namespace SticksAndStonesGCApi.Services
{
    public interface IGolfCourseService
    {
        Task<GolfCourse> GetGolfCourseAsync(string courseName);
    }

    public class GolfCourseService : IGolfCourseService
    {
        private readonly ILogger<GolfCourseService> _logger;
        private readonly ISourceRepo _repo;

        public GolfCourseService(ILogger<GolfCourseService> logger, ISourceRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<GolfCourse> GetGolfCourseAsync(string courseName)
        {
            var sourceCourse = await _repo.GetCourseAsync(courseName);

            var course = sourceCourse != null ? new GolfCourse
            {
                Id = sourceCourse.Id,
                Name = sourceCourse.Name,
                Holes = sourceCourse.Holes,
                Scorecard = sourceCourse.Scorecard,
                TeeBoxes = sourceCourse.TeeBoxes,
                CreatedAt = sourceCourse.CreatedAt,
                UpdatedAt = sourceCourse.UpdatedAt
            } : new GolfCourse();

            return course;
        }
    }
}
