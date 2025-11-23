using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SticksAndStonesGCApi.Models
{
    public class GolfCourse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Holes { get; set; }
        public List<ScorecardItem>? Scorecard { get; set; }

        public List<TeeBox>? TeeBoxes { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
