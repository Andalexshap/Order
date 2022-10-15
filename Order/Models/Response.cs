namespace Order.Models
{
    public class Response
    {
        public bool Sucsess { get; set; }
        public List<Error> Error { get; set; }
    }
}
