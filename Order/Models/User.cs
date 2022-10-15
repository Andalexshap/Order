namespace Order.Models
{
    public class User
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Key { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MeddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public Address? Address { get; set; }
        public MemberType? MemberType { get; set; }

        public void SetUser(User user)
        {
            Login = user.Login;
            Password = user.Password;
            Key = user.Key;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            MeddleName = user.MeddleName;
            PhoneNumber = user.PhoneNumber;
            Address = user.Address;
            MemberType = user.MemberType;
        }
        public void Logout()
        {
            Login = null;
            Password = null;
            Key = null;
            Email = null;
            FirstName = null;
            LastName = null;
            MeddleName = null;
            PhoneNumber = null;
            Address = null;
            MemberType = null;
        }
    }
}
