using System;
using System.Collections.Generic;
using System.Linq;

namespace PalengKart
{
    public class SalesReport
    {
        public List<Sale> Sales { get; set; }

        public SalesReport()
        {
            Sales = new List<Sale>();
        }

        public void RecordSale(string customerUsername, string productName, int quantity, decimal totalAmount)
        {
            Sales.Add(new Sale(Sales.Count + 1, customerUsername, productName, quantity, totalAmount));
        }

        public void DisplaySalesReport()
        {
            Console.WriteLine("\n--- SALES REPORT ---");
            if (Sales.Count == 0)
            {
                Console.WriteLine("(no sales yet)");
                return;
            }
            foreach (var s in Sales)
            {
                Console.WriteLine($"#{s.SaleID,-4} {s.Date:yyyy-MM-dd HH:mm} | {s.CustomerUsername,-12} | {s.ProductName,-20} x{s.Quantity,-4} | ₱{s.TotalAmount,10:0.00}");
            }
            Console.WriteLine($"TOTAL SALES: ₱{Sales.Sum(s => s.TotalAmount):0.00}");
        }
    }
}
