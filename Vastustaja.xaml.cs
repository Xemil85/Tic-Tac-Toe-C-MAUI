using System.Text.Json;

namespace final_work;

public partial class Vastustaja : ContentPage
{
    private Pelaaja pelaaja;

    [Obsolete]
    public Vastustaja(Pelaaja pelaaja)
	{
		InitializeComponent();
        this.pelaaja = pelaaja;
        Tietokone.Clicked += Tietokone_Clicked;
        ToinenPelaaja.Clicked += ToinenPelaaja_Clicked;
	}

    [Obsolete]
    private async void Tietokone_Clicked(object sender, EventArgs e)
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
                Ristinolla ristinollaPage = new Ristinolla(pelaaja, tietokonePelaaja, true);
                await Navigation.PushAsync(ristinollaPage);
            }
            else
            {
                // Tietokonepelaajaa ei l�ytynyt luoda uusi tietokonepelaaja ja lis�t� se pelaajien listaan ja tallentaa JSON-tiedostoon
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

                pelaajat.Add(tietokonepelaaja);
                string updatedData = JsonSerializer.Serialize(pelaajat);

                try
                {
                    // Tallentaa tiedot json tiedostoon
                    System.IO.File.WriteAllText(filePath, updatedData);
                }
                catch (Exception ex)
                {
                    // N�yt� virheilmoitus
                    await DisplayAlert("Virhe tallennuksessa", ex.Message, "OK");
                }

                // Luo ristinolla-n�kym� ja siirry siihen antaen pelaajatiedot
                Ristinolla ristinollaPage = new Ristinolla(pelaaja, tietokonepelaaja, true);
                await Navigation.PushAsync(ristinollaPage);
            }
        }
    }

    // Funktio joka mahdollistaa toimivuuden toinen pelaajan valitsemisen n�kym��n
    [Obsolete]
    private async void ToinenPelaaja_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ToinenPelaaja(pelaaja));
    }
}