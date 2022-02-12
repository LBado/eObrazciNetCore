using eObrazci.Models;
using System.ComponentModel.DataAnnotations;

namespace eObrazci.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Priimek { get; set; } = string.Empty;
        public string Spol { get; set; } = string.Empty;
        public DateTime DatumRojstva { get; set; }
        public NaslovDTO Naslov { get; set; }
        public ICollection<IzpitDTO>? Izpit { get; set; }
    }
}
