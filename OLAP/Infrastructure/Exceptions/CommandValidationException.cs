namespace OLAP.API.Infrastructure.Exceptions
{
    public class CommandValidationException : Exception
    {
        public List<KeyValuePair<string, string>> Args { get; set; }
        public CommandValidationException()
        { }

        public CommandValidationException(string message)
            : base(message)
        { }

        public CommandValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
        public CommandValidationException(string message, List<KeyValuePair<string, string>> args, Exception innerException)
            : base(message, innerException)
        {
            Args = args ?? new List<KeyValuePair<string, string>>();
        }
    }
}
