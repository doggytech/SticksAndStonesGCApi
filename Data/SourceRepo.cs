using SticksAndStonesGCApi.Models;

namespace SticksAndStonesGCApi.Data
{
    public interface ISourceRepo
    {
        Task<List<SourceData>> GetDataFromSourceAsync();
        Task<SourceData> GetCourseAsync(string courseName);
    }
    public class SourceRepo : ISourceRepo
    {
        private readonly LocalCache _cache;
        private readonly ILogger<SourceRepo> _logger;

        public SourceRepo(LocalCache cache, ILogger<SourceRepo> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<SourceData>> GetDataFromSourceAsync()
        {
            // Try cache first
            var cached = _cache.GetCourses();

            if (cached != null && cached.Count > 0)
            {
                return cached;
            }

            return new List<SourceData>();
        }

        public async Task<SourceData> GetCourseAsync(string courseName)
        {
            var cachedCourse = _cache.GetCourse(courseName);

            if (cachedCourse != null && cachedCourse.Name != string.Empty)
            {
                _logger.LogInformation("Course {CourseName} found in cache.", courseName);

                return cachedCourse;
            }

            _logger.LogInformation("Course {CourseName} not found in cache. Building request for source API.", courseName);

            string body = string.Empty;
            var response = new HttpResponseMessage();
            var client = new HttpClient();
            var request = new HttpRequestMessage();

            string sourceApiKey = Environment.GetEnvironmentVariable("SOURCE_API_KEY") ?? "";
            string sourceApiHost = Environment.GetEnvironmentVariable("SOURCE_API_HOST") ?? "";

            var encodedName = Uri.EscapeDataString(courseName ?? string.Empty);

            request.RequestUri = new Uri($"https://golf-course-api.p.rapidapi.com/search?name={encodedName}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("x-rapidapi-key", sourceApiKey);
            request.Headers.Add("x-rapidapi-host", sourceApiHost);

            try
            {
                _logger.LogInformation("Fetching course data for {CourseName} from source API.", courseName);

                response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                body = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Successfully fetched course data for {CourseName} from source API.", courseName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching course data for {CourseName}. Error: {ex.Message}", courseName, ex.Message);
            }

            // Deserialize JSON into model
            _logger.LogInformation("Deserializing course data for {CourseName}.", courseName);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var resultsList = System.Text.Json.JsonSerializer.Deserialize<List<SourceData>>(body, options) ?? new List<SourceData>();
            SourceData result = resultsList.FirstOrDefault() ?? new SourceData();

            // persist to cache
            _logger.LogInformation("Saving course data for {CourseName} to cache.", courseName);

            try
            {
                if (result.Name == null || result.Name == string.Empty)
                {
                    _logger.LogWarning("No course data found for {CourseName}", courseName);
                }
                else
                {
                    _cache.SaveCourse(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving course data to cache for {CourseName}. Error: {ex.Message}", courseName, ex.Message);
            }

            _logger.LogInformation("Successfully saved course data for {CourseName}.", courseName);

            return result;
        }
    }
}
