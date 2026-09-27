using System;

namespace PalengKart
{
    public class Admin : User
    {
        public Admin(string username, string email) : base(username, email)
        {
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[Admin] {Username} ({Email})");
        }
    }
}
