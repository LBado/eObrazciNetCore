using System.ComponentModel.DataAnnotations;

namespace eObrazci.Models
{
    public class Student
    {
        public int Id { get; set; } 
        public string Ime { get; set; } = string.Empty;
        public string Priimek { get; set; } = string.Empty;
        public string Spol { get; set; } = string.Empty;
        public DateTime DatumRojstva { get; set; }
        public Naslov Naslov { get; set; } 
        public ICollection<Izpit>? Izpit { get; set; }
        public int ObrazecId { get; set; }
        public Obrazec Obrazec { get; set; }
    }
}
