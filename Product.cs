namespace PalengKart
{
    public class Product
    {
        public string Barcode { get; set; } = "";
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        
        public Product(string barcode, string name, string category, decimal price, int stock, int minStock)
        {
            Barcode = barcode;
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
            MinStock = minStock;
        }
        
        public bool IsLowStock => Stock <= MinStock;
    }
}
