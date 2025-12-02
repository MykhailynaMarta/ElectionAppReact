using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionAppReact.Server.Models
{
    [Table("candidates")]

    public class Candidate
    {
        public int id { get; set; }
        public string name { get; set; }
        public string party { get; set; }
        public string description { get; set; }
        public DateTime birthdate { get; set; }
        public string photourl { get; set; }
    }
}
