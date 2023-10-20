using System.Text.Json;

namespace final_work;

public partial class Vastustaja : ContentPage
{
    private Pelaaja pelaaja;
    public Vastustaja(Pelaaja pelaaja)
	{
		InitializeComponent();
        this.pelaaja = pelaaja;
        Tietokone.Clicked += Tietokone_Clicked;
        ToinenPelaaja.Clicked += ToinenPelaaja_Clicked;
	}

    private void Tietokone_Clicked(object sender, EventArgs e)
    {
        string projectDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Pelaajat.json";
        string filePath = System.IO.Path.Combine(projectDirectory, fileName);

        if (System.IO.File.Exists(filePath))
        {
            string existingData = System.IO.File.ReadAllText(filePath);
            List<Pelaaja> pelaajat = JsonSerializer.Deserialize<List<Pelaaja>>(existingData);

            // Etsi tietokonepelaaja
            Pelaaja tietokonePelaaja = pelaajat.FirstOrDefault(p => p.Etunimi == "Tietokone");

            if (tietokonePelaaja != null)
            {
                // L�ytyi tietokonepelaaja, voit k�ytt�� sit� vastustajana
                // Luo ristinolla-n�kym� ja siirry siihen antaen pelaajatiedot
                Ristinolla ristinollaPage = new Ristinolla(pelaaja, tietokonePelaaja);
                Navigation.PushAsync(ristinollaPage);
            }
            else
            {
                // Tietokonepelaajaa ei l�ytynyt, voit k�sitell� t�t� tilannetta tarpeidesi mukaan
                // Esim. luoda uusi tietokonepelaaja ja lis�t� se pelaajien listaan ja tallentaa JSON-tiedostoon
                Pelaaja tietokonepelaaja = new Pelaaja
                {
                    Etunimi = "Tietokone",
                    Sukunimi = "Pelaaja",
                    Syntymavuosi = 0,
                    Voitot = 0,
                    Tappiot = 0,
                    Tasapelit = 0,
                    PelienYhteiskesto = 0
                };

                // Luo ristinolla-n�kym� ja siirry siihen antaen pelaajatiedot
                Ristinolla ristinollaPage = new Ristinolla(pelaaja, tietokonepelaaja);
                Navigation.PushAsync(ristinollaPage);
            }
        }
        else
        {
            // Tiedostoa ei ole olemassa, voit k�sitell� t�t� tilannetta tarpeidesi mukaan
            // Esim. luoda tiedosto ja lis�t� tietokonepelaajan
        }
    }

    private async void ToinenPelaaja_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ToinenPelaaja(pelaaja));
    }
}