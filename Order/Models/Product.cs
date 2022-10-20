namespace Order.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Photo { get; set; }
        public int? Quantity { get; set; }
        public int? Inventory { get; set; }
    }
}
