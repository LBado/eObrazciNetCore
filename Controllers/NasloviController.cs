using eObrazci.Interfaces;
using eObrazci.Models;
using Microsoft.AspNetCore.Mvc;

namespace eObrazci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NasloviController : Controller
    {
        public INaslovRepository _naslovRepository { get; }
        public NasloviController(INaslovRepository naslovRepository)
        {
            _naslovRepository = naslovRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Naslov>))]
        public IActionResult GetNaslovi()
        {
            var naslovi = _naslovRepository.GetNaslovi();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(naslovi);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Naslov))]
        [ProducesResponseType(400)]
        public IActionResult GetNaslov(int id)
        {
            if (!_naslovRepository.NaslovExists(id))
                return BadRequest(ModelState);

            var naslov = _naslovRepository.GetNaslov(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(naslov);

        }

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(200, Type = typeof(Naslov))]
        [ProducesResponseType(400)]
        public IActionResult GetNaslovByStudentId(int studentId)
        {
            if (!_naslovRepository.NaslovByStudentIdExists(studentId))
                return BadRequest(ModelState);

            var naslov = _naslovRepository.GetNaslovByStudentId(studentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(naslov);
        }

    }
}
