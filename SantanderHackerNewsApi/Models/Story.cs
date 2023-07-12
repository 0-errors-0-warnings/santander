namespace SantanderHackerNewsApi.Models;

public class Story
{
    public string? Title { get; set; }
    public string? Type { get; set; }
    public string? Url { get; set; }
    public string? By { get; set; }
    public int? Score { get; set; }
    public int? Id { get; set; }
    public long? Time { get; set; }
    public DateTime? DateTime { get
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds((double)Time).ToLocalTime();
            return dateTime;
        }
    }
}
