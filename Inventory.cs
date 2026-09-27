using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;

namespace PalengKart
{
    public class Inventory
    {
        public List<Product> Products { get; set; }
        string filePath = "inventory.txt";

        public Inventory()
        {
            Products = new List<Product>();
            LoadInventory();
        }

        public void AddProduct(Product p)
        {
            Products.Add(p);
            SaveInventory();
        }

        public void RemoveProduct(string productID)
        {
            Products.RemoveAll(p => p.ProductID == productID);
            SaveInventory();
        }

        public void UpdateProduct(string productID, decimal price, int quantity)
        {
            var p = GetProduct(productID);
            if (p != null)
            {
                p.Price = price;
                p.Quantity = quantity;
                SaveInventory();
            }
        }

        public void DisplayInventory()
        {
            Console.WriteLine("\n--- INVENTORY ---");
            if (Products.Count == 0)
            {
                Console.WriteLine("(no products)");
                return;
            }
            foreach (var p in Products.OrderBy(p => p.Category).ThenBy(p => p.Name))
            {
                p.DisplayProduct();
            }
        }

        public Product? GetProduct(string productID)
        {
            return Products.FirstOrDefault(p => p.ProductID == productID);
        }

        public bool ProductExists(string productID)
        {
            return GetProduct(productID) != null;
        }

        // Takes quantity out of stock; returns false if there isn't enough
        public bool ReduceStock(string productID, int quantity)
        {
            var p = GetProduct(productID);
            if (p == null || quantity <= 0 || p.Quantity < quantity) return false;

            p.Quantity -= quantity;
            SaveInventory();
            return true;
        }

        public List<Product> GetLowStock()
        {
            return Products.Where(p => p.IsLowStock).ToList();
        }

        public void SaveInventory()
        {
            using (StreamWriter w = new StreamWriter(filePath))
            {
                foreach (var p in Products)
                {
                    // Invariant culture so the file loads the same on any PC regional setting
                    w.WriteLine(string.Join("|",
                        Clean(p.ProductID),
                        Clean(p.Name),
                        p.Category,
                        p.Price.ToString(CultureInfo.InvariantCulture),
                        p.Quantity.ToString(CultureInfo.InvariantCulture),
                        Clean(p.Unit),
                        p.MinStock.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }

        public void LoadInventory()
        {
            if (!File.Exists(filePath)) return;

            Products.Clear();
            int lineNo = 0;
            foreach (var line in File.ReadAllLines(filePath))
            {
                lineNo++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                if (parts.Length >= 7
                    && Enum.TryParse(parts[2], out Category category)
                    && decimal.TryParse(parts[3], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price)
                    && int.TryParse(parts[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out int quantity)
                    && int.TryParse(parts[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out int minStock))
                {
                    Products.Add(new Product(parts[0], parts[1], category, price, quantity, parts[5], minStock));
                }
                else
                {
                    Console.WriteLine($"Warning: skipped invalid line {lineNo} in {filePath}");
                }
            }
        }

        // '|' is the field separator in the save file, so it can't appear inside a value
        static string Clean(string value)
        {
            return value.Replace("|", "/").Replace("\r", " ").Replace("\n", " ");
        }
    }
}
