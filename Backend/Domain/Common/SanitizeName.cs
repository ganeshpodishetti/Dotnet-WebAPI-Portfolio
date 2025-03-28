using System.Text.RegularExpressions;

namespace Domain.Common;

public static partial class SanitizeName
{
    public interface ISanitizeName
    {
        bool SanitizeNameAsync(string name);
    }

    public partial class SanitizeNameService : ISanitizeName
    {
        public bool SanitizeNameAsync(string name)
        {
            // Remove special characters and spaces
            var sanitizeName = MyRegex().Replace(name, "");
            // Check for special characters and spaces
            return name == sanitizeName;
        }

        // Regex for special characters and spaces
        [GeneratedRegex(@"[^a-zA-Z0-9.]")]
        private static partial Regex MyRegex();
    }
}