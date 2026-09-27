using System;
using System.Linq;
using PalengKart;

namespace PalengKartApp
{
    class Program
    {
        static Inventory inventory = new Inventory();
        static SalesReport salesReport = new SalesReport();
        static Admin admin = new Admin("admin", "admin@palengkart.com");

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (inventory.Products.Count == 0)
            {
                Console.WriteLine("Adding sample products...");
                AddSample("Rice", Category.PastaRiceCereals, 55, 100, "kg", 20);
                AddSample("Cooking Oil", Category.SaucesCondiments, 120, 30, "bottle", 5);
                AddSample("Eggs", Category.Dairy, 9, 60, "pc", 12);
                AddSample("Tomato", Category.Vegetables, 80, 15, "kg", 5);
                AddSample("Banana", Category.Fruit, 70, 4, "kg", 5);
            }

            bool run = true;
            while (run)
            {
                Console.WriteLine("\n=== PALENGKART STORE SYSTEM ===");
                Console.WriteLine("1. Login as Admin");
                Console.WriteLine("2. Exit");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Admin username: ");
                        if (Console.ReadLine()?.Trim() == admin.Username)
                            HandleAdmin(admin);
                        else
                            PrintError("✗ Unknown admin username!");
                        break;
                    case "2":
                        inventory.SaveInventory();
                        PrintSuccess("\n✓ Data saved. Thank you for using PalengKart!");
                        run = false;
                        break;
                    default:
                        PrintError("Invalid option! Please try again.");
                        break;
                }
            }
        }

        // ===== ADMIN =====

        static void HandleAdmin(Admin user)
        {
            Console.WriteLine();
            user.DisplayInfo();

            while (true)
            {
                Console.WriteLine("\n--- ADMIN MENU ---");
                Console.WriteLine("1. View Inventory");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Remove Product");
                Console.WriteLine("5. Low Stock Alerts");
                Console.WriteLine("6. Sales Report");
                Console.WriteLine("7. Logout");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1": DisplayInventory(); break;
                    case "2": AddProduct(); break;
                    case "3": UpdateProduct(); break;
                    case "4": RemoveProduct(); break;
                    case "5": ShowLowStock(); break;
                    case "6": salesReport.DisplaySalesReport(); break;
                    case "7": return;
                    default: PrintError("Invalid option! Please try again."); break;
                }
            }
        }

        static void DisplayInventory()
        {
            inventory.DisplayInventory();
        }

        static void AddProduct()
        {
            string id = BarcodeGenerator.GenerateBarcodeNumber(inventory.ProductExists);
            Console.WriteLine("\nGenerated Product ID (barcode): " + id);
            BarcodeGenerator.DisplayBarcodeAscii(id);

            Console.Write("Product Name: ");
            string? name = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(name)) name = "Unknown";

            Category category = ReadCategory();
            decimal price = ReadDecimal("Price: ₱", 0);
            string unit = ReadUnit();
            int quantity = ReadInt("Quantity: ", 0);
            int minStock = ReadInt("Minimum Stock (low stock alert level): ", 0);

            inventory.AddProduct(new Product(id, name, category, price, quantity, unit, minStock));
            PrintSuccess("✓ Product added successfully!");
        }

        static void UpdateProduct()
        {
            var prod = FindProduct("\nEnter Product ID: ");
            if (prod == null) return;

            Console.Write("Current: ");
            prod.DisplayProduct();

            decimal newPrice = prod.Price;
            int newQuantity = prod.Quantity;

            while (true)
            {
                Console.Write("New Price (press Enter to skip): ");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) break;
                if (decimal.TryParse(input, out decimal parsed) && parsed >= 0) { newPrice = parsed; break; }
                Console.WriteLine("Please enter a valid number (at least 0).");
            }
            while (true)
            {
                Console.Write("New Quantity (press Enter to skip): ");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) break;
                if (int.TryParse(input, out int parsed) && parsed >= 0) { newQuantity = parsed; break; }
                Console.WriteLine("Please enter a valid whole number (at least 0).");
            }

            inventory.UpdateProduct(prod.ProductID, newPrice, newQuantity);
            PrintSuccess("✓ Product updated!");
        }

        static void RemoveProduct()
        {
            var prod = FindProduct("\nEnter Product ID: ");
            if (prod == null) return;

            Console.Write($"Remove {prod.Name}? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() == "Y")
            {
                inventory.RemoveProduct(prod.ProductID);
                PrintSuccess("✓ Product removed!");
            }
        }

        static void ShowLowStock()
        {
            var low = inventory.GetLowStock();
            if (low.Count == 0)
            {
                PrintSuccess("\n✓ All products have sufficient stock!");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️ LOW STOCK ALERTS:");
            Console.ResetColor();
            foreach (var p in low)
            {
                Console.WriteLine($"  • {p.Name} - Qty: {p.Quantity} {p.Unit} (Min: {p.MinStock})");
            }
        }

        // ===== HELPERS =====

        static void AddSample(string name, Category category, decimal price, int quantity, string unit, int minStock)
        {
            string id = BarcodeGenerator.GenerateBarcodeNumber(inventory.ProductExists);
            inventory.AddProduct(new Product(id, name, category, price, quantity, unit, minStock));
        }

        static Product? FindProduct(string prompt)
        {
            Console.Write(prompt);
            string? id = Console.ReadLine()?.Trim();
            var prod = string.IsNullOrEmpty(id) ? null : inventory.GetProduct(id);
            if (prod == null) PrintError("✗ Product not found!");
            return prod;
        }

        static Category ReadCategory()
        {
            var categories = Enum.GetValues<Category>();
            Console.WriteLine("Categories:");
            for (int i = 0; i < categories.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {categories[i]}");
            }
            int choice = ReadInt("Category number: ", 1, categories.Length);
            return categories[choice - 1];
        }

        static string ReadUnit()
        {
            while (true)
            {
                Console.Write($"Unit ({string.Join(", ", Product.ValidUnits)}): ");
                string unit = Console.ReadLine()?.Trim() ?? "";
                if (Product.IsValidUnit(unit))
                    return Product.ValidUnits.First(u => u.Equals(unit, StringComparison.OrdinalIgnoreCase));
                Console.WriteLine("Please enter one of the listed units.");
            }
        }

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

        static int ReadInt(string prompt, int min, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;
                Console.WriteLine(max == int.MaxValue
                    ? $"Please enter a valid whole number (at least {min})."
                    : $"Please enter a whole number from {min} to {max}.");
            }
        }

        static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
