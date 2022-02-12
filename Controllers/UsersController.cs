using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eObrazci.Data;
using eObrazci.Models;
using eObrazci.Interfaces;
using eObrazci.DTO;
using System.Security.Cryptography;
using AutoMapper;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace eObrazci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UsersController(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        //remove when in production!
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpPost("register")]
        public IActionResult Register(UserDTO request)
        {
            if (_userRepository.UserExists(request.Username))
            {
                return BadRequest("Username already exists!");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            //save user
            var userMap = _mapper.Map<User>(request);
            userMap.PasswordHash = passwordHash;
            userMap.PasswordSalt = passwordSalt;

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created!");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, User user)
        {
            
            using(var hmac = new HMACSHA512(user.PasswordSalt))
            {
                //naredimo nov computedHash z podanim username in usersalt iz db,
                //novi hash se mora ujemati z tistim v bazi
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        //mockup login
        //[HttpGet("login")]
        //public IActionResult UserVerify(string username, string password)
        //{
        //    if (!_userRepository.UserExists(username)) 
        //        return NotFound();

        //    var isVerified = _userRepository.UserVerify(username, password);

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    return Ok(isVerified);

        //}

        [HttpPost("login")]
        public IActionResult UserLogin(UserDTO user)
        {
            if (!_userRepository.UserExists(user.Username))
                return NotFound();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbUser = _userRepository.GetUserByUsername(user.Username);

            if(!VerifyPasswordHash(user.Password, dbUser))
            {
                return BadRequest("Wrong password");
            }

            //create token
            string token = CreateToken(dbUser);

            return Ok(token);

        }

        private string CreateToken(User user)
        {
            //claims so properties, opisuje userja ki je authenticated(to je lahko lahko ime, id, email etc.)
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            //v key damo secret token(ki smo ga dali v appsettings.json)
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            //credentials ki jih podpišemo z keyom
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //payload
            var token = new JwtSecurityToken(
                claims: claims, 
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: cred
                );

            //jwt v format v stringu(hitro lahko delamo z njim)
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
