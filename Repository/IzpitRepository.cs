using eObrazci.Data;
using eObrazci.Interfaces;
using eObrazci.Models;

namespace eObrazci.Repository
{
    public class IzpitRepository : IIzpitRepository
    {
        public readonly DataContext _context;
        public IzpitRepository(DataContext context)
        {
            _context = context;
        }

        public Izpit GetIzpit(int id)
        {
            return _context.Izpiti.Where(izpit=>izpit.Id == id).FirstOrDefault();   
        }

        public ICollection<Izpit> GetIzpiti()
        {
            return _context.Izpiti.OrderBy(izpit => izpit.Id).ToList();
        }

        public ICollection<Izpit> GetIzpitiByStudentId(int studentId)
        {
            return _context.Izpiti
                .Where(izpit=>izpit.StudentId == studentId)
                .OrderBy(izpit => izpit.Id)
                .ToList();
        }

        public bool IzpitExists(int id)
        {
            return _context.Izpiti.Any(izpit => izpit.Id == id);
        }

        public bool IzpitiExistsByStudentId(int studentId)
        {
            return _context.Izpiti.Any(izpit => izpit.StudentId == studentId);
        }
    }
}
