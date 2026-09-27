using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;

namespace PalengKart
{
    public class Inventory
    {
        public List<Product> Products = new List<Product>();
        string filePath = "inventory.txt";

        public void AddProduct(Product p)
        {
            Products.Add(p);
            SaveInventory();
        }

        public void RemoveProduct(string barcode)
        {
            Products.RemoveAll(p => p.Barcode == barcode);
            SaveInventory();
        }

        public void UpdateProduct(string barcode, decimal price, int stock)
        {
            var p = GetProduct(barcode);
            if (p != null)
            {
                p.Price = price;
                p.Stock = stock;
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
            foreach (var p in Products)
            {
                Console.WriteLine(p.Barcode + " | " + p.Name + " | " + p.Category + " | ₱" + p.Price + " | Stock: " + p.Stock);
            }
        }

        public bool SellProduct(string barcode, int qty, out decimal total)
        {
            total = 0;
            if (qty <= 0) return false;

            var p = GetProduct(barcode);
            if (p != null && p.Stock >= qty)
            {
                p.Stock -= qty;
                total = p.Price * qty;
                SaveInventory();
                return true;
            }
            return false;
        }

        public void SaveInventory()
        {
            using (StreamWriter w = new StreamWriter(filePath))
            {
                foreach (var p in Products)
                {
                    // Invariant culture so the file loads the same on any PC regional setting
                    w.WriteLine(string.Join("|",
                        Clean(p.Barcode),
                        Clean(p.Name),
                        Clean(p.Category),
                        p.Price.ToString(CultureInfo.InvariantCulture),
                        p.Stock.ToString(CultureInfo.InvariantCulture),
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
                if (parts.Length >= 6
                    && decimal.TryParse(parts[3], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price)
                    && int.TryParse(parts[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out int stock)
                    && int.TryParse(parts[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out int minStock))
                {
                    Products.Add(new Product(parts[0], parts[1], parts[2], price, stock, minStock));
                }
                else
                {
                    Console.WriteLine($"Warning: skipped invalid line {lineNo} in {filePath}");
                }
            }
        }

        public Product? GetProduct(string barcode)
        {
            return Products.FirstOrDefault(p => p.Barcode == barcode);
        }

        public bool BarcodeExists(string barcode)
        {
            return GetProduct(barcode) != null;
        }

        public List<Product> GetLowStock()
        {
            return Products.Where(p => p.IsLowStock).ToList();
        }

        // '|' is the field separator in the save file, so it can't appear inside a value
        static string Clean(string value)
        {
            return value.Replace("|", "/").Replace("\r", " ").Replace("\n", " ");
        }
    }
}
