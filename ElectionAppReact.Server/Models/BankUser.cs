namespace ElectionAppReact.Server.Models
{
    public class BankUser
    {
        public int Id { get; set; }

        public string Bank { get; set; }

        public string FullName { get; set; }       
        public DateTime BirthDate { get; set; }    
        public string Address { get; set; }        
        public string Password { get; set; }
    }
}
