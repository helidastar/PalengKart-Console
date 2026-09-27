using System;
using PalengKart;

namespace PalengKartApp
{
    class Program
    {
        static decimal ReadDecimal(string prompt, decimal min)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= min)
                    return value;
                Console.WriteLine($"Please enter a valid number (at least {min}).");
            }
        }

        static int ReadInt(string prompt, int min)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min)
                    return value;
                Console.WriteLine($"Please enter a valid whole number (at least {min}).");
            }
        }

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Inventory inv = new Inventory();
            inv.LoadInventory();
            
            if (inv.Products.Count == 0)
            {
                Console.WriteLine("Adding sample products...");
                inv.AddProduct(new Product("8901234567890", "Rice 5kg", "Groceries", 250, 50, 10));
                inv.AddProduct(new Product("8901234567891", "Cooking Oil", "Groceries", 120, 30, 5));
                inv.AddProduct(new Product("8901234567892", "Sugar 1kg", "Groceries", 75, 40, 8));
            }
            
            bool run = true;
            while (run)
            {
                Console.WriteLine("\n=== PALENGKART STORE SYSTEM ===");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. Sell Product");
                Console.WriteLine("4. Update Product");
                Console.WriteLine("5. Remove Product");
                Console.WriteLine("6. Low Stock Alerts");
                Console.WriteLine("7. Exit");
                Console.Write("Choice: ");
                
                string? c = Console.ReadLine();
                
                if (c == "1")
                {
                    inv.DisplayInventory();
                }
                else if (c == "2")
                {
                    string b = BarcodeGenerator.GenerateBarcodeNumber(inv.BarcodeExists);
                    Console.WriteLine("\nGenerated Barcode: " + b);
                    BarcodeGenerator.DisplayBarcodeAscii(b);
                    Console.Write("Product Name: ");
                    string? n = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(n)) n = "Unknown";
                    Console.Write("Category (press Enter for General): ");
                    string? cat = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(cat)) cat = "General";
                    decimal p = ReadDecimal("Price: ₱", 0);
                    int s = ReadInt("Stock: ", 0);
                    int min = ReadInt("Minimum Stock (low stock alert level): ", 0);
                    inv.AddProduct(new Product(b, n, cat, p, s, min));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✓ Product added successfully!");
                    Console.ResetColor();
                }
                else if (c == "3")
                {
                    Console.Write("\nScan/Enter Barcode: ");
                    string? b = Console.ReadLine();
                    if (string.IsNullOrEmpty(b))
                    {
                        Console.WriteLine("Invalid barcode!");
                    }
                    else
                    {
                        var prod = inv.GetProduct(b);
                        if (prod != null)
                        {
                            Console.WriteLine($"Product: {prod.Name}");
                            Console.WriteLine($"Price: ₱{prod.Price}");
                            Console.WriteLine($"Available Stock: {prod.Stock}");
                            int q = ReadInt("Quantity: ", 1);
                            if (inv.SellProduct(b, q, out decimal total))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"✓ Sold! Total: ₱{total}");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("✗ Insufficient stock!");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("✗ Product not found!");
                            Console.ResetColor();
                        }
                    }
                }
                else if (c == "4")
                {
                    Console.Write("\nEnter Barcode: ");
                    string? b = Console.ReadLine();
                    if (string.IsNullOrEmpty(b))
                    {
                        Console.WriteLine("Invalid barcode!");
                    }
                    else
                    {
                        var prod = inv.GetProduct(b);
                        if (prod != null)
                        {
                            Console.WriteLine($"Current: {prod.Name} - ₱{prod.Price} - Stock: {prod.Stock}");
                            decimal newPrice = prod.Price;
                            int newStock = prod.Stock;

                            while (true)
                            {
                                Console.Write("New Price (press Enter to skip): ");
                                string? priceInput = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(priceInput)) break;
                                if (decimal.TryParse(priceInput, out decimal parsed) && parsed >= 0) { newPrice = parsed; break; }
                                Console.WriteLine("Please enter a valid number (at least 0).");
                            }
                            while (true)
                            {
                                Console.Write("New Stock (press Enter to skip): ");
                                string? stockInput = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(stockInput)) break;
                                if (int.TryParse(stockInput, out int parsed) && parsed >= 0) { newStock = parsed; break; }
                                Console.WriteLine("Please enter a valid whole number (at least 0).");
                            }
                            
                            inv.UpdateProduct(b, newPrice, newStock);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("✓ Product updated!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("✗ Product not found!");
                            Console.ResetColor();
                        }
                    }
                }
                else if (c == "5")
                {
                    Console.Write("\nEnter Barcode: ");
                    string? b = Console.ReadLine();
                    if (string.IsNullOrEmpty(b))
                    {
                        Console.WriteLine("Invalid barcode!");
                    }
                    else
                    {
                        var prod = inv.GetProduct(b);
                        if (prod != null)
                        {
                            Console.WriteLine($"Remove {prod.Name}? (Y/N): ");
                            if (Console.ReadLine()?.ToUpper() == "Y")
                            {
                                inv.RemoveProduct(b);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("✓ Product removed!");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("✗ Product not found!");
                            Console.ResetColor();
                        }
                    }
                }
                else if (c == "6")
                {
                    var low = inv.GetLowStock();
                    if (low.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n✓ All products have sufficient stock!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n⚠️ LOW STOCK ALERTS:");
                        Console.ResetColor();
                        foreach (var p in low)
                        {
                            Console.WriteLine($"  • {p.Name} - Stock: {p.Stock} (Min: {p.MinStock})");
                        }
                    }
                }
                else if (c == "7")
                {
                    inv.SaveInventory();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n✓ Data saved. Thank you for using PalengKart!");
                    Console.ResetColor();
                    run = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option! Please try again.");
                    Console.ResetColor();
                }
                
                if (run)
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
            }
        }
    }
}
