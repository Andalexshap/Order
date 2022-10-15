namespace Order.Models.Order
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
