using System.ComponentModel.DataAnnotations;

namespace eObrazci.DTO
{
    public class IzpitDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public DateTime DatumOprIzpita { get; set; }
        [Range(1, 10, ErrorMessage ="Dovoljene ocene od {1} do {2}")]
        public int Ocena { get; set; }
    }
}
