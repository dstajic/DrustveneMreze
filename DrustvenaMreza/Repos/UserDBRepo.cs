using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;


namespace DrustvenaMreza.Repos
{
    public class UserDBRepo
    {

        private string connectionString = "Data Source=Data/UserGroups.db";

        public List<Korisnik> GetAll()
        {
            List<Korisnik> korisnici = new List<Korisnik>();

            using SqliteConnection connection = new SqliteConnection(connectionString);
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
        public Korisnik GetByID(int id)
        {


            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM Users WHERE Id = @Id";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Korisnik user = new Korisnik(Convert.ToInt32(reader["Id"]),
                      reader["Username"].ToString(),
                      reader["Name"].ToString(),
                      reader["Surname"].ToString(),
                      DateTime.ParseExact(reader["Birthday"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return user;
            }
            return null;

           
        }
    }

}
