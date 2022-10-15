namespace Order.Models
{
    public enum OrderStatus
    {
        Issued,
        NotPayment,
        Paid,
        Confirmed,
        Fulfilled,
        Canceled,
        Error
    }
}
