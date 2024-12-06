namespace Contracts.Authentication
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }

        public IEnumerable<string> Errors { get; set; }

    }

}
