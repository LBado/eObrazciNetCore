using eObrazci.Data;
using eObrazci.Interfaces;
using eObrazci.Models;

namespace eObrazci.Repository
{
    public class NaslovRepository : INaslovRepository
    {
        private readonly DataContext _context;

        public NaslovRepository(DataContext dataContext)
        {
            _context = dataContext;

        }
        public Naslov GetNaslov(int id)
        {
            return _context.Naslovi.Where(naslov => naslov.Id == id).FirstOrDefault();
        }

        public Naslov GetNaslovByStudentId(int studentId)
        {
            return _context.Naslovi.Where(naslov => naslov.StudentId == studentId).FirstOrDefault();    
        }

        public ICollection<Naslov> GetNaslovi()
        {
            return _context.Naslovi.OrderBy(naslov => naslov.Id).ToList();
        }

        public bool NaslovByStudentIdExists(int studentId)
        {
            return _context.Naslovi.Any(naslov => naslov.StudentId == studentId);
        }

        public bool NaslovExists(int id)
        {
            return _context.Naslovi.Any(naslov => naslov.Id == id);
        }
    }
}
