using DrustvenaMreza.Models;

namespace DrustvenaMreza.Repos
{
    public class KorisniciRepo
    {
        public const string putanja = "Data/korisnici.csv";
        public static Dictionary<int, Korisnik> Data;

        public KorisniciRepo()
        {
            if(Data == null)
            {
                Load();
            }
        }
        public void Load()
        {
            Data = new Dictionary<int, Korisnik>();
            string [] lines = File.ReadAllLines(putanja);
            foreach(var line in lines)
            {
                string[] detalji = line.Split(',');
                int id = int.Parse(detalji[0]);
                string username = detalji[1];
                string ime = detalji[2];
                string prezime = detalji[3];
                DateTime datumRodjenja = DateTime.Parse(detalji[4]);
                
                Korisnik korisnik = new Korisnik(id, username, ime, prezime, datumRodjenja);
                Data[id] = korisnik;
            }
        }
        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Korisnik r in Data.Values)
            {
                lines.Add($"{r.id},{r.username},{r.ime},{r.prezime},{r.datumRodjenja}");
            }
            File.WriteAllLines(putanja, lines);
        }
    }
}
