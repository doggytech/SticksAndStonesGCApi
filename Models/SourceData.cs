using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LiteDB;

namespace SticksAndStonesGCApi.Models
{
    public class SourceData
    {
        [BsonId]
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        public List<string>? Likes { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }

        public string? Country { get; set; }

        public string? Coordinates { get; set; }

        public string? Holes { get; set; }

        public string? LengthFormat { get; set; }

        public string? GreenGrass { get; set; }

        public string? FairwayGrass { get; set; }

        public List<ScorecardItem>? Scorecard { get; set; }

        public List<TeeBox>? TeeBoxes { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("__v")]
        public int Version { get; set; }
        public string? message { get; set; }
    }

    public class ScorecardItem
    {
        public int Hole { get; set; }

        public int Par { get; set; }

        // The tees object has dynamic keys like "teeBox1"; map as dictionary
        public Dictionary<string, TeeBoxDetails>? Tees { get; set; }

        public int Handicap { get; set; }
    }

    public class TeeBoxDetails
    {
        [JsonPropertyName("color")]
        public string? Color { get; set; }

        [JsonPropertyName("yards")]
        public int Yards { get; set; }
    }

    public class TeeBox
    {
        [JsonPropertyName("tee")]
        public string? TeeName { get; set; }

        public int Slope { get; set; }

        public double Handicap { get; set; }
    }
}
