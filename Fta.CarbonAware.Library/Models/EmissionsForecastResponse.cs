using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace Fta.CarbonAware.Library.Models
{
    [Serializable]
    public class EmissionsForecastResponse
    {
        [JsonPropertyName("generatedAt")]
        public DateTimeOffset GeneratedAt { get; set; }

        [JsonPropertyName("requestedAt")]
        public DateTimeOffset RequestedAt { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; } = default!;

        [JsonPropertyName("dataStartAt")]
        public DateTimeOffset DataStartAt { get; set; }
        
        [JsonPropertyName("dataEndAt")]
        public DateTimeOffset DataEndAt { get; set; }
        
        [JsonPropertyName("windowSize")]
        public int WindowSize { get; set; }
        
        [JsonPropertyName("optimalDataPoints")]
        public IEnumerable<DataPoint> OptimalDataPoints { get; set; } = new List<DataPoint>();
        
        [JsonPropertyName("forecastData")]
        public IEnumerable<DataPoint> ForecastData { get; set; } = new List<DataPoint>();
    }

    [Serializable]
    public class DataPoint
    {
        [JsonPropertyName("location")]
        public string Location { get; set; } = default!;

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
        
        [JsonPropertyName("duration")]
        public int Duration { get; set; }
        
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }
}
