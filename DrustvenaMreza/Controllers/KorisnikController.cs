using DrustvenaMreza.Models;
using DrustvenaMreza.Repos;
using Microsoft.AspNetCore.Http;
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

        /*[HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            KorisniciRepo korisniciRepo = new KorisniciRepo();

            List<Korisnik> Korisnici = KorisniciRepo.Data.Values.ToList();
            return Ok(Korisnici);
        }*/
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            try
            {
                List<Korisnik> Korisnici = GetAllFromDataBase();
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

            KorisniciRepo korisniciRepo = new KorisniciRepo();
            if (!KorisniciRepo.Data.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(KorisniciRepo.Data[id]);
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
        private List<Korisnik> GetAllFromDataBase()
        {
            List<Korisnik> korisnici = new List<Korisnik>();

            using SqliteConnection connection = new SqliteConnection("DataSource=Data/UserGroups.db");
            connection.Open();

            string query = "SELECT * FROM Users";
            using SqliteCommand command = new SqliteCommand(query, connection);
            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                korisnici.Add(new Korisnik(
                      Convert.ToInt32(reader["Id"]),
                      reader["Username"].ToString(),
                      reader["Name"].ToString(),
                      reader["Surname"].ToString(),
                      DateTime.ParseExact(reader["Birthday"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                ));
            }



            return korisnici;

        }
    }

}
