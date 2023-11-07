using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using static final_work.Ristinolla;

namespace final_work;

public partial class UusiPelaaja : ContentPage
{
    public UusiPelaaja()
	{
		InitializeComponent();
        TallennaPelaaja.Clicked += TallennaPelaaja_Clicked;
	}

    private async void TallennaPelaaja_Clicked(object sender, EventArgs e)
    {
        // Tarkistaa onko entryt tyhji� ja jos on niin tulee virheilmoitus ett� sy�t� kaikki tiedot.
        // Jolloin pelaajan tietoja ei viel� tallenneta.
        if (string.IsNullOrEmpty(Etunimi.Text) || string.IsNullOrEmpty(Sukunimi.Text) || string.IsNullOrEmpty(Syntymavuosi.Text))
        {
            await DisplayAlert("Virhe", "Sy�t� kaikki tiedot ennen tallentamista.", "OK");
            return;
        }

        Pelaaja pelaaja = new Pelaaja
        {
            Etunimi = Etunimi.Text,
            Sukunimi = Sukunimi.Text,
            Syntymavuosi = int.Parse(Syntymavuosi.Text),
            Voitot = 0,
            Tappiot = 0,
            Tasapelit = 0,
            PelienYhteiskesto = 0
        };

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // Muunnetaan pelaajatieto JSON-muotoon

        string projectDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Pelaajat.json";
        string filePath = System.IO.Path.Combine(projectDirectory, fileName);

        List<Pelaaja> pelaajat;

        if (System.IO.File.Exists(filePath))
        {
            // Lue tiedoston nykyinen sis�lt� ja deserialisoi se pelaajien listaksi
            string existingData = System.IO.File.ReadAllText(filePath);
            pelaajat = JsonSerializer.Deserialize<List<Pelaaja>>(existingData);
        }
        else
        {
            // Luo uusi lista, jos tiedostoa ei ole viel� olemassa
            pelaajat = new List<Pelaaja>();
        }

        // Lis�� uusi pelaaja listalle
        pelaajat.Add(pelaaja);

        // Muunnetaan p�ivitetty tieto takaisin JSON-muotoon
        string updatedData = JsonSerializer.Serialize(pelaajat, options);

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

        // Tyhjenn� Entry-kent�t
        Etunimi.Text = string.Empty;
        Sukunimi.Text = string.Empty;
        Syntymavuosi.Text = string.Empty;

        // L�het� uuden pelaajan tiedot ja menee vastustaja n�kym��n.
        Vastustaja vastustaja = new Vastustaja(pelaaja);
        await Navigation.PushAsync(vastustaja);
    }
}