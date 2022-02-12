using AutoMapper;
using eObrazci.DTO;
using eObrazci.Interfaces;
using eObrazci.Models;
using Microsoft.AspNetCore.Mvc;

namespace eObrazci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {

        //student repository je tipa IStudent (ima GetStudents) ...specificiramo
        //ko injectamo dependency specificiramo StudentRepository
        //new StudentController(StudentRepository) bo rabil StudentRepository
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        //IEnumerable je podobno kot collection. Je list
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
        public IActionResult GetStudents()
        {
            //namesto da importamo context v konstruktorju in hardcodamo kar imamo kar smo
            //ze naredili v repositoriju -> importamo repositori
            //mappamo V DTO, vsak student bo zgledal kot DTO
            var students = _mapper.Map<List<StudentDTO>>(_studentRepository.GetStudents());

            //ModelState je v kakšnem state-u je model/ če postamo/requestamo napačne podatke
            //npr namest student damo Dog al pa kej tazga (je tip validacije)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(students);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type= typeof(Student))]
        [ProducesResponseType(400)]
        public IActionResult GetStudent(int id)
        {
            //preverimo če obstaja
            if (!_studentRepository.StudentExists(id))
                return NotFound();

            var student = _mapper.Map<StudentDTO>(_studentRepository.GetStudent(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(student);
        }

        [HttpGet("{id}/povpOcena")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetStudentPovpOcena(int id)
        {
            if (!_studentRepository.StudentExists(id))
                return NotFound();

            var povpOcena = _studentRepository.GetStudentPovpOcena(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(povpOcena);

        }

        [HttpGet("ime/{ime}")]
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(400)]
        public IActionResult GetStudentByIme(string ime)
        {
            //preverimo če obstaja
            if (!_studentRepository.StudentByImeExists(ime))
                return NotFound();

            var student = _mapper.Map<StudentDTO>(_studentRepository.GetStudent(ime));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(student);
        }
    }
}
