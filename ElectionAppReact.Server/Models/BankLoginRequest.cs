namespace ElectionAppReact.Server.Models
{
    public class BankLoginRequest
    {
        public string Bank { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }

        // пароль у ключі від банку
        public string PasswordFromBank { get; set; }

        // пароль, який вводить користувач з клавіатури
        public string UserEnteredPassword { get; set; }
    }
}
