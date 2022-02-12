using eObrazci.Data;
using eObrazci.Interfaces;
using eObrazci.Models;
using Microsoft.EntityFrameworkCore;

namespace eObrazci.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;
        //s konstruktorjem naredimo dataContext context 
        public StudentRepository(DataContext context)
        {
            _context = context;
        }

        public Student GetStudent(int id)
        {
            //Where je SQL abstrakcija, FirstOrDefault vrne prvega ki najde
            return _context.Students
                .Where(s => s.Id == id)
                .Include(s => s.Naslov)
                .Include(s=>s.Izpit)
                .FirstOrDefault();
        }

        public Student GetStudent(string ime)
        {
            return _context.Students.Where(s => s.Ime == ime).FirstOrDefault();
        }

        public Student GetStudentByObrazecId(int obrazecId)
        {
            return _context.Students.Where(s => s.ObrazecId == obrazecId).FirstOrDefault();
        }

        public Student GetStudentDefault(int id)
        {
            return _context.Students.Where(s => s.Id == id).FirstOrDefault();
        }

        public decimal GetStudentPovpOcena(int id)
        {
            var izpiti = _context.Izpiti.Where(i => i.StudentId == id);

            if (!izpiti.Any()) return 0;

            //decimal naredi konverzijo
            return ((decimal)izpiti.Sum(i => i.Ocena) / izpiti.Count());
        }

        //returnamo ICollection of students
        public ICollection<Student> GetStudents()
        {
            //glede na dbSet returnamo Studente. Moramo dati ToList(da bo list)
            //ker imamo ICollection
            return _context.Students.OrderBy(student => student.Id).ToList();
        }

        public bool StudentByImeExists(string ime)
        {
            return _context.Students.Any(student => student.Ime == ime);
        }

        bool IStudentRepository.StudentExists(int id)
        {
            return _context.Students.Any(student => student.Id == id);
        }
    }
}
