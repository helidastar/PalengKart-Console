using System;

namespace PalengKart
{
    public class Customer : User
    {
        public string Password { get; set; }
        public ShoppingCart Cart { get; set; } = new ShoppingCart();

        public Customer(string username, string email, string password) : base(username, email)
        {
            Password = password;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[Customer] {Username} ({Email}) - {Cart.Products.Count} item(s) in cart");
        }
    }
}
