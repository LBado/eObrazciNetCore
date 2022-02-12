using System.ComponentModel.DataAnnotations;

namespace eObrazci.DTO
{
    public class NaslovDTO
    {
        public int Id { get; set; }
        public string Ulica { get; set; } = string.Empty;
        public string HisnaStevilka { get; set; } = string.Empty;
        [Range(1000, 9999, ErrorMessage = "Postna stevilka ni pravilnega formata")]
        public int PostnaStevilka { get; set; }
        public string Kraj { get; set; } = string.Empty;
        public string Drzava { get; set; } = string.Empty;
    }
}
