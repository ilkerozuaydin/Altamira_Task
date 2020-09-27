namespace Core.Utilities.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult() : base(false)
        {
        }

        public ErrorResult(bool success, string message) : base(false, message)
        {
        }

        public ErrorResult(string message) : base(false)
        {
        }
    }
}