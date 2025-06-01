using DrustvenaMreza.Models;
using DrustvenaMreza.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace DrustvenaMreza.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {

        
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            UserDBRepo userDBRepo = new UserDBRepo();
            try
            {
                List<Korisnik> Korisnici = userDBRepo.GetAll();
                return Ok(Korisnici);
            }

            catch (SqliteException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                return BadRequest($"Data formatting error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            try
            {
                UserDBRepo userDBRepo = new UserDBRepo();
                Korisnik user = userDBRepo.GetByID(id);

                if (user == null)
                {
                    return NotFound(); 
                }

                return Ok(user); 
            }
            catch (SqliteException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                return BadRequest($"Data formatting error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }

        }
        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik newKorisnik)
        {
            UserDBRepo userDBRepo = new UserDBRepo();

            if (string.IsNullOrWhiteSpace(newKorisnik.username) || string.IsNullOrWhiteSpace(newKorisnik.ime) || string.IsNullOrWhiteSpace(newKorisnik.prezime))
            {
                return BadRequest();
            }
            try
            {
                userDBRepo.CreateNewUser(newKorisnik);
                return Ok(newKorisnik);
            }
            catch (SqliteException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                return BadRequest($"Data formatting error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            UserDBRepo userDBRepo = new UserDBRepo();
            try
            {
                userDBRepo.DeleteById(id);
                return NoContent();
            }
            catch (SqliteException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                return BadRequest($"Data formatting error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik uKorisnik)
        {
            try
            {
                UserDBRepo userDBRepo = new UserDBRepo();
                userDBRepo.UpdateByID(id,uKorisnik);
                uKorisnik.id = id;
                return Ok(uKorisnik);
            }
            catch (SqliteException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                return BadRequest($"Data formatting error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }
        }

        }

}
