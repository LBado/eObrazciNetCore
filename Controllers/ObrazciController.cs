using AutoMapper;
using eObrazci.Data;
using eObrazci.DTO;
using eObrazci.Interfaces;
using eObrazci.Models;
using Microsoft.AspNetCore.Mvc;

namespace eObrazci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObrazciController : Controller
    {
        private readonly IObrazecRepository _obrazecRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IIzpitRepository _izpitRepository;
        private readonly INaslovRepository _naslovRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ObrazciController(
            IObrazecRepository obrazecRepository, 
            IStudentRepository studentRepository,
            IIzpitRepository izpitRepository,
            INaslovRepository naslovRepository,
            DataContext context,
            IMapper mapper 
            ) 
        {
            _obrazecRepository = obrazecRepository;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _izpitRepository = izpitRepository;
            _naslovRepository = naslovRepository;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Obrazec>))]
        public IActionResult GetObrazci()
        {
            var obrazci = _mapper.Map<List<ObrazecDTO>>(_obrazecRepository.GetObrazci());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(obrazci);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Obrazec))]
        [ProducesResponseType(400)]
        public IActionResult GetObrazec(int id)
        {
            
            if(!_obrazecRepository.ObrazecExists(id))
                return NotFound();

            var obrazec = _obrazecRepository.GetObrazec(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(obrazec);
        }

        [HttpGet("{id}/detail")]
        [ProducesResponseType(200, Type = typeof(Obrazec))]
        [ProducesResponseType(400)]
        public IActionResult GetObrazecDetail(int id)
        {
            if (!_obrazecRepository.ObrazecExists(id))
                return NotFound();

            var obrazec = _mapper.Map<ObrazecDTO>(_obrazecRepository.GetObrazecDetail(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(obrazec);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateObrazec([FromBody] StudentDTO student)
        {
            if (student == null)
                return BadRequest(ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            //mappamo iz DTO v Student da lahko creatamo obrazec z studentom
            var studentMap = _mapper.Map<Student>(student);

            var country = _obrazecRepository.GetCountryNameByISOPublicSOAP(student.Naslov.Drzava);

            if (country.Result == null || country.Result == "error")
            {
                ModelState.AddModelError("", "Country not found");
                return StatusCode(500, ModelState);
            }
            //ISO v string
            studentMap.Naslov.Drzava = country.Result; 

            if (!_obrazecRepository.CreateObrazec(studentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{obrazecId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateObrazec(int obrazecId, [FromBody] StudentDTO updatedStudent)
        {
            if (updatedStudent == null)
                return BadRequest(ModelState);

            if (obrazecId == null)
                return BadRequest(ModelState);

            if (!_obrazecRepository.ObrazecExists(obrazecId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingObrazec = _obrazecRepository.GetObrazec(obrazecId);

            var existingStudent = _studentRepository.GetStudent(updatedStudent.Id);

            //Soap update country
            var country = _obrazecRepository.GetCountryNameByISOPublicSOAP(updatedStudent.Naslov.Drzava);

            if (country.Result == null || country.Result == "error")
            {
                ModelState.AddModelError("", "Country not found");
                return StatusCode(500, ModelState);
            }

            updatedStudent.Naslov.Drzava = country.Result;

            _context.Entry(existingStudent).CurrentValues.SetValues(updatedStudent);

            var existingNaslov = _naslovRepository.GetNaslov(updatedStudent.Naslov.Id);

            _context.Entry(existingNaslov).CurrentValues.SetValues(updatedStudent.Naslov);

            foreach (var existingIzpit in existingObrazec.Student.Izpit.ToList())
            {
                //delete izpit (če ga ni v novi tabeli)
                if(!updatedStudent.Izpit.Any(i=>i.Id == existingIzpit.Id))
                    _context.Izpiti.Remove(existingIzpit);
            }

            foreach (var izpit in updatedStudent.Izpit)
            {
                var existingIzpit = _izpitRepository.GetIzpit(izpit.Id);

                if (existingIzpit != null)
                {
                    //update
                    _context.Entry(existingIzpit).CurrentValues.SetValues(izpit);
                } else
                {
                    //add
                    var newIzpit = new Izpit()
                    {
                        Naziv = izpit.Naziv,
                        DatumOprIzpita = izpit.DatumOprIzpita,
                        Ocena = izpit.Ocena,
                    };

                    existingObrazec.Student.Izpit.Add(newIzpit);
                }
            }

            if (!_obrazecRepository.UpdateObrazec(existingObrazec))
            {
                ModelState.AddModelError("", "Something went wrong while updating!");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated!");

        }

        [HttpDelete("{obrazecId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteObrazec(int obrazecId)
        {
            if (obrazecId == null)
                return BadRequest(ModelState);

            if (!_obrazecRepository.ObrazecExists(obrazecId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingObrazec = _obrazecRepository.GetObrazec(obrazecId);

            if (!_obrazecRepository.DeleteObrazec(existingObrazec))
            {
                ModelState.AddModelError("", "Something went wrong while deleting!");
                return StatusCode(500, ModelState);

            }

            return Ok("Successfully deleted!");
        }

    }
}
