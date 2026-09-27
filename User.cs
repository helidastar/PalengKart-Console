namespace PalengKart
{
    public abstract class User
    {
        public string Username { get; set; }
        public string Email { get; set; }

        protected User(string username, string email)
        {
            Username = username;
            Email = email;
        }

        public abstract void DisplayInfo();
    }
}
