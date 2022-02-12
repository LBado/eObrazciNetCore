using eObrazci.Models;

namespace eObrazci.Interfaces
{
    public interface IIzpitRepository
    {
        ICollection<Izpit> GetIzpiti();
        Izpit GetIzpit(int id);
        ICollection<Izpit> GetIzpitiByStudentId(int studentId);
        bool IzpitExists(int id);
        bool IzpitiExistsByStudentId(int studentId);
    }
}
