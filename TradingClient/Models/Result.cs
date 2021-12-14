namespace TradingClient.Models
{
    public struct Result
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        public Result()
        {
            Success = true;
        }

        public Result(string err)
        {
            ErrorMessage = err;
        }
    }
}