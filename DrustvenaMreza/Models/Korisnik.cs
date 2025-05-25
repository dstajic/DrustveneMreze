namespace DrustvenaMreza.Models
{
    public class Korisnik
    {
        public int id {  get; set; }
        public string username { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public DateTime datumRodjenja { get; set; }

        public Korisnik(int id, string username, string ime, string prezime, DateTime datumRodjenja) { 
            this.id = id;
            this.username = username;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = datumRodjenja;
        }
    }
}
