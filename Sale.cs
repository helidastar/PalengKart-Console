using System;

namespace PalengKart
{
    public class Sale
    {
        public int SaleID { get; set; }
        public string CustomerUsername { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }

        public Sale(int saleID, string customerUsername, string productName, int quantity, decimal totalAmount)
        {
            SaleID = saleID;
            CustomerUsername = customerUsername;
            ProductName = productName;
            Quantity = quantity;
            TotalAmount = totalAmount;
            Date = DateTime.Now;
        }
    }
}
