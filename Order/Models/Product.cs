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

        public void Merge(Product inputProduct, int? qty)
        {
            if (!string.IsNullOrEmpty(inputProduct.Name))
            {
                Name = inputProduct.Name;
            }

            if (!string.IsNullOrEmpty(inputProduct.Description))
            {
                Description = inputProduct.Description;
            }

            if (inputProduct.Price != null)
            {
                Price = inputProduct.Price;
            }

            if (!string.IsNullOrEmpty(inputProduct.Photo))
            {
                Photo = inputProduct.Photo;
            }

            if (inputProduct.Inventory != null)
            {
                Inventory = inputProduct.Inventory;
            }

            if (Inventory != null & qty != null)
            {
                Inventory += qty;
            }
        }
    }
}
