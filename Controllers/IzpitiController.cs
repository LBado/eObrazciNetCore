using eObrazci.Interfaces;
using eObrazci.Models;
using Microsoft.AspNetCore.Mvc;

namespace eObrazci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IzpitiController : Controller
    {
        public IIzpitRepository _izpitRepository;
        public IzpitiController(IIzpitRepository izpitRepository)
        {
            _izpitRepository = izpitRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Izpit>))]
        public IActionResult GetIzpiti()
        {
            var izpiti= _izpitRepository.GetIzpiti();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(izpiti);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Izpit))]
        [ProducesResponseType(400)]
        public IActionResult GetIzpit(int id)
        {

            if (!_izpitRepository.IzpitExists(id))
                return NotFound();

            var izpit = _izpitRepository.GetIzpit(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(izpit);
        }

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Izpit>))]
        [ProducesResponseType(400)]
        public IActionResult GetIzpitiByStudentId(int studentId)
        {

            if (!_izpitRepository.IzpitiExistsByStudentId(studentId))
                return NotFound();

            var izpiti = _izpitRepository.GetIzpitiByStudentId(studentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(izpiti);
        }

    }
}
