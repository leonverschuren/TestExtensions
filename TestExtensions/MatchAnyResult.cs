namespace TestExtensions
{
    public class MatchAnyResult
    {
        public bool IsMatch { get; }
        public string Message { get; }

        private MatchAnyResult(bool isMatch, string message)
        {
            IsMatch = isMatch;
            Message = message;
        }

        internal static MatchAnyResult CreateMatch()
        {
            return new(true, string.Empty);
        }

        internal static MatchAnyResult CreateNoMatch(string message)
        {
            return new(false, message);
        }
    }
}
