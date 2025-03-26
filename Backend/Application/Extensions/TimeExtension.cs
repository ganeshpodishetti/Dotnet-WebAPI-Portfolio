namespace Application.Extensions;

public static class TimeExtensions
{
    public static string TimesAgo(this DateTime dateTime)
    {
        string result;
        var timeSpan = DateTime.UtcNow.Subtract(dateTime);

        if (timeSpan <= TimeSpan.FromSeconds(60))
            result = $"{timeSpan.Seconds} seconds ago";
        else if (timeSpan <= TimeSpan.FromMinutes(60))
            result = timeSpan.Minutes > 1 ? $"{timeSpan.Minutes} minutes ago" : "about a minute ago";
        else if (timeSpan <= TimeSpan.FromHours(24))
            result = timeSpan.Hours > 1 ? $"{timeSpan.Hours} hours ago" : "about an hour ago";
        else if (timeSpan <= TimeSpan.FromDays(30))
            result = timeSpan.Days > 1 ? $"{timeSpan.Days} days ago" : "yesterday";
        else if (timeSpan <= TimeSpan.FromDays(365))
            result = timeSpan.Days > 30 ? $"{timeSpan.Days / 30} months ago" : "about a month ago";
        else
            result = timeSpan.Days > 365 ? $"{timeSpan.Days / 365} years ago" : "a year ago";

        return result;
    }
}