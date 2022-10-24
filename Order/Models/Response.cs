namespace Order.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public List<Error> Errors { get; set; }
    }
}
