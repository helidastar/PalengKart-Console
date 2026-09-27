using System;
using System.Linq;

namespace PalengKart
{
    public class Product
    {
        public static readonly string[] ValidUnits = { "pc", "kg", "g", "L", "mL", "pack", "bottle", "can", "sack", "dozen" };

        public string ProductID { get; set; } = "";   // EAN-13 barcode
        public string Name { get; set; } = "";
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; } = "pc";
        public int MinStock { get; set; }

        public Product(string productID, string name, Category category, decimal price, int quantity, string unit, int minStock)
        {
            ProductID = productID;
            Name = name;
            Category = category;
            Price = price;
            Quantity = quantity;
            Unit = unit;
            MinStock = minStock;
        }

        public bool IsLowStock => Quantity <= MinStock;

        public static bool IsValidUnit(string unit)
        {
            return ValidUnits.Contains(unit, StringComparer.OrdinalIgnoreCase);
        }

        public void DisplayProduct()
        {
            Console.WriteLine($"{ProductID} | {Name,-20} | {Category,-20} | ₱{Price,8:0.00} / {Unit,-6} | Qty: {Quantity}");
        }
    }
}
