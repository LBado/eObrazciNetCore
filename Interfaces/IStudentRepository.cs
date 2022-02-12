using eObrazci.Models;

namespace eObrazci.Interfaces
{
    public interface IStudentRepository
    {
        //List studentov
        ICollection<Student> GetStudents();
        //detail api spodaj
        //En student
        Student GetStudent(int id);
        Student GetStudentDefault(int id);
        Student GetStudent(string ime);
        Student GetStudentByObrazecId(int obrazecId);
        bool StudentExists(int id);
        bool StudentByImeExists(string ime);
        decimal GetStudentPovpOcena(int id);
        
    }
}
