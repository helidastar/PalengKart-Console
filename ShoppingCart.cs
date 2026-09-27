using System;
using System.Collections.Generic;
using System.Linq;

namespace PalengKart
{
    public class ShoppingCart
    {
        // Each entry is a copy of the product, where Quantity is the amount in the cart
        public List<Product> Products { get; set; }

        public ShoppingCart()
        {
            Products = new List<Product>();
        }

        public void AddToCart(Product product, int quantity)
        {
            var existing = Products.FirstOrDefault(p => p.ProductID == product.ProductID);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                Products.Add(new Product(product.ProductID, product.Name, product.Category,
                    product.Price, quantity, product.Unit, 0));
            }
        }

        public int QuantityOf(string productID)
        {
            return Products.Where(p => p.ProductID == productID).Sum(p => p.Quantity);
        }

        public decimal CalculateTotal()
        {
            return Products.Sum(p => p.Price * p.Quantity);
        }

        public void DisplayCart()
        {
            Console.WriteLine("\n--- YOUR CART ---");
            if (Products.Count == 0)
            {
                Console.WriteLine("(cart is empty)");
                return;
            }
            foreach (var p in Products)
            {
                Console.WriteLine($"{p.Name,-20} {p.Quantity,4} {p.Unit,-6} x ₱{p.Price,8:0.00} = ₱{p.Price * p.Quantity,10:0.00}");
            }
            Console.WriteLine($"TOTAL: ₱{CalculateTotal():0.00}");
        }

        public void Clear()
        {
            Products.Clear();
        }
    }
}
