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
                      DateTime.Parse(reader["Birthday"].ToString(), CultureInfo.InvariantCulture)
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
                      DateTime.Parse(reader["Birthday"].ToString(), CultureInfo.InvariantCulture));
                return user;
            }
            return null;

           
        }
        public void CreateNewUser(Korisnik user)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string query = $"INSERT INTO Users (Username, Name, Surname, Birthday) VALUES(@Username, @Name, @Surname, @Birthday)";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Username", user.username);
            command.Parameters.AddWithValue("@Name", user.ime);
            command.Parameters.AddWithValue("@Surname", user.prezime);
            command.Parameters.AddWithValue("@Birthday", user.datumRodjenja.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();

        
        }
        public void DeleteById(int id)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string query = $"DELETE FROM Users WHERE Id=@Id";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
        public void UpdateByID(int id,Korisnik user)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string query = @"UPDATE Users 
                     SET Username = @Username, 
                         Name = @Name, 
                         Surname = @Surname, 
                         Birthday = @Birthday 
                     WHERE Id = @Id";
            using SqliteCommand command = new SqliteCommand( query, connection);
            command.Parameters.AddWithValue("@Username", user.username);
            command.Parameters.AddWithValue("@Name", user.ime);
            command.Parameters.AddWithValue("@Surname", user.prezime);
            command.Parameters.AddWithValue("@Birthday", user.datumRodjenja.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();

        }
    }

}
