namespace Order.Models.Account
{
    public class AuthorizationResponse : Response
    {
        public AuthorizationResponse()
        {
        }

        public AuthorizationResponse(bool success)
        {
            Success = success;
        }

        public AuthorizationResponse(Error error)
        {
            Success = false;
            Errors = new List<Error> { error };
        }

        public AuthorizationResponse(User user)
        {
            Success = true;
            User = user;
        }

        public User? User { get; set; }
    }
}
