using eObrazci.Models;

namespace eObrazci.Interfaces
{
    public interface INaslovRepository
    {
        ICollection<Naslov> GetNaslovi();
        Naslov GetNaslov(int id);

        Naslov GetNaslovByStudentId(int studentId);

        bool NaslovExists(int id);

        bool NaslovByStudentIdExists(int studentId);
    }
}
