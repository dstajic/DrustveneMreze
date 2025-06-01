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
            KorisniciRepo korisniciRepo = new KorisniciRepo();
            if (string.IsNullOrWhiteSpace(newKorisnik.username) || string.IsNullOrWhiteSpace(newKorisnik.ime) || string.IsNullOrWhiteSpace(newKorisnik.prezime))
            {
                return BadRequest();
            }

            newKorisnik.id = SracunajNovID(KorisniciRepo.Data.Keys.ToList());
            KorisniciRepo.Data[newKorisnik.id] = newKorisnik;
            korisniciRepo.Save();

            return Ok(newKorisnik);
        }
        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik uKorisnik)
        {
            KorisniciRepo korisniciRepo = new KorisniciRepo();
            if (string.IsNullOrWhiteSpace(uKorisnik.username) || string.IsNullOrWhiteSpace(uKorisnik.ime) || string.IsNullOrWhiteSpace(uKorisnik.prezime))
            {
                return BadRequest();
            }
            if (!KorisniciRepo.Data.ContainsKey(id))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisniciRepo.Data[id];
            korisnik.username = uKorisnik.username;
            korisnik.ime = uKorisnik.ime;
            korisnik.prezime = uKorisnik.prezime;
            korisniciRepo.Save();

            return Ok(korisnik);
        }
        private int SracunajNovID(List<int> Idevi)
        {
            int maxId = 0;
            foreach (int id in Idevi)
            {
                if (id > maxId)
                {
                    maxId = id;
                }

            }
            return maxId + 1;
        }
       
    }

}
