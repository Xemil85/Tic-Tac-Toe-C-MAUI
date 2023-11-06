using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Timers;

namespace final_work;

public partial class Ristinolla : ContentPage
{
    // Tarvittavat muuttujat ristinolla peli‰ varten.
    private Pelaaja pelaaja1;
    private Pelaaja pelaaja2;
    private bool pelaaja1Vuoro = true;
    private List<Button> buttons;
    private string[] board;
    private DateTime pelinAlkuaika;
    private double pelienYhteiskesto = 0;
    private bool peliJatkuu = false;
    private bool pelikelloKaynnistetty = false;
    int pisteet = 0;

    private bool tietokoneVastustaja = false;

    [Obsolete]
    public Ristinolla(Pelaaja pelaaja1, Pelaaja pelaaja2, bool tietokoneVastustaja)
    {
        InitializeComponent();
        this.pelaaja1 = pelaaja1;
        this.pelaaja2 = pelaaja2;
        this.tietokoneVastustaja = tietokoneVastustaja;

        buttons = new List<Button> { Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8 };
        board = new string[9];

        foreach (var button in buttons)
        {
            button.Clicked += OnButtonClick;
        }

        PelaajaVuoro.Text = $"Pelaajan {pelaaja1.Etunimi} {pelaaja1.Sukunimi} vuoro";

    }

    // Funktio tietokone vastustajan toimivuus
    [Obsolete]
    private async void tietokoneSiirto()
    {
        // Tietokone tekee siirron 0.5-2s sis‰ll‰
        Random random = new Random();
        int delaySeconds = random.Next(500, 2001);
        await Task.Delay(delaySeconds);

        // Tarkistaa mitk‰ napeista on valmiina.
        List<int> vapaaIndeksit = new List<int>();
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == null)
            {
                vapaaIndeksit.Add(i);
            }
        }

        // Valitsee vapaan napin ja tulee siihen ristinollan arvo.
        if (vapaaIndeksit.Count > 0)
        {
            int valittuIndeksi = vapaaIndeksit[random.Next(0, vapaaIndeksit.Count)];
            OnButtonClick(buttons[valittuIndeksi], EventArgs.Empty);
        }
    }

    [Obsolete]
    private void KaynnistaPelikello()
    {
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (peliJatkuu)
            {
                TimeSpan kulunutAika = DateTime.Now - pelinAlkuaika;
                Aika.Text = "Aika: " + kulunutAika.ToString(@"mm\:ss");

                pelienYhteiskesto = kulunutAika.TotalSeconds;
                Debug.WriteLine($"Pelien yhteiskesto on {pelienYhteiskesto}");
            }
            return true;
        });
    }
    // Funktio, joka mahdollistaa nappien painalluksen ja ristinolla pelin etenemisen.
    [Obsolete]
    private async void OnButtonClick(object sender, EventArgs e)
    {
        peliJatkuu = true;

        if (!pelikelloKaynnistetty)
        {
            pelikelloKaynnistetty = true;
            pelinAlkuaika = DateTime.Now;
            KaynnistaPelikello(); // K‰ynnist‰ pelikello vasta kun ensimm‰inen nappi painetaan
        }

        var button = (Button)sender;
        int index = buttons.IndexOf(button);

        string projectDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Pelaajat.json";
        string filePath = Path.Combine(projectDirectory, fileName);

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        if (File.Exists(filePath))
        {

            if (board[index] == null)
            {
                string jsonData = File.ReadAllText(filePath);
                ObservableCollection<Pelaaja> pelaajat = JsonSerializer.Deserialize<ObservableCollection<Pelaaja>>(jsonData);

                // Etsi ja p‰ivit‰ pelaajien tiedot
                var pelaaja1ToUpdate = pelaajat.FirstOrDefault(p => p.Etunimi == pelaaja1.Etunimi && p.Sukunimi == pelaaja1.Sukunimi);
                var pelaaja2ToUpdate = pelaajat.FirstOrDefault(p => p.Etunimi == pelaaja2.Etunimi && p.Sukunimi == pelaaja2.Sukunimi);

                // P‰‰tt‰‰ pelaajan vuoron ja napin arvon.
                if (pelaaja1Vuoro)
                {
                    PelaajaVuoro.Text = $"Pelaajan {pelaaja2.Etunimi} {pelaaja2.Sukunimi} vuoro";
                    button.Text = "X";
                    board[index] = "X";
                }
                else
                {
                    PelaajaVuoro.Text = $"Pelaajan {pelaaja1.Etunimi} {pelaaja1.Sukunimi} vuoro";
                    button.Text = "O";
                    board[index] = "O";
                }

                pelaaja1Vuoro = !pelaaja1Vuoro;

                // Tarkistaa kumpi pelaaja voitti ja p‰ivitet‰‰n json tiedostoon oikeat arvot.
                if (CheckForWin("X"))
                {
                    pisteet = int.Parse(Pelaaja1Pisteet.Text);
                    Pelaaja1Pisteet.Text = (pisteet + 1).ToString();
                    pelaaja1ToUpdate.Voitot++;
                    pelaaja2ToUpdate.Tappiot++;
                    pelaaja1ToUpdate.PelienYhteiskesto += pelienYhteiskesto;
                    pelaaja2ToUpdate.PelienYhteiskesto += pelienYhteiskesto;
                    peliJatkuu = false;

                    // Kirjoita p‰ivitetyt tiedot takaisin JSON-tiedostoon
                    string updatedJsonData = JsonSerializer.Serialize(pelaajat, options);
                    File.WriteAllText(filePath, updatedJsonData);

                    // Ponnahdusikkuna, joka n‰ytt‰‰ kumpi voitta ja kysyy haluatko jatkaa peli‰.
                    bool continueGame = await DisplayAlert("Peli p‰‰ttyi", $"Pelaaja {pelaaja1.Etunimi} {pelaaja1.Sukunimi} voitti! Haluatko pelata uudestaan?", "Kyll‰", "Lopeta");
                    if (continueGame)
                    {
                        // Jatka peli‰
                        ResetGame();
                    }
                    else
                    {
                        // Lopettaa pelin ja menn‰‰n takaisin aloitusruutuun
                        ResetGame();
                        await Navigation.PushAsync(new MainPage());
                    }

                }

                else if (CheckForWin("O"))
                {
                    pisteet = int.Parse(Pelaaja2Pisteet.Text);
                    Pelaaja2Pisteet.Text = (pisteet + 1).ToString();
                    pelaaja2ToUpdate.Voitot++;
                    pelaaja1ToUpdate.Tappiot++;
                    pelaaja1ToUpdate.PelienYhteiskesto += pelienYhteiskesto;
                    pelaaja2ToUpdate.PelienYhteiskesto += pelienYhteiskesto;
                    peliJatkuu = false;
                    // Kirjoita p‰ivitetyt tiedot takaisin JSON-tiedostoon
                    string updatedJsonData = JsonSerializer.Serialize(pelaajat, options);
                    File.WriteAllText(filePath, updatedJsonData);

                    // Ponnahdusikkuna, joka n‰ytt‰‰ kumpi voitta ja kysyy haluatko jatkaa peli‰.
                    bool continueGame = await DisplayAlert("Peli p‰‰ttyi", $"Pelaaja {pelaaja2.Etunimi} {pelaaja2.Sukunimi} voitti! Haluatko pelata uudestaan?", "Kyll‰", "Lopeta");
                    if (continueGame)
                    {
                        // Jatka peli‰
                        ResetGame();
                    }
                    else
                    {
                        // Lopeta peli ja menn‰‰n takaisin aloitusruutuun
                        ResetGame();
                        await Navigation.PushAsync(new MainPage());
                    }
                }
                else if (board.All(cell => cell != null))
                {
                    pelaaja1ToUpdate.Tasapelit++;
                    pelaaja2ToUpdate.Tasapelit++;
                    pelaaja1ToUpdate.PelienYhteiskesto += pelienYhteiskesto;
                    pelaaja2ToUpdate.PelienYhteiskesto += pelienYhteiskesto;
                    peliJatkuu = false;
                    // Kirjoita p‰ivitetyt tiedot takaisin JSON-tiedostoon
                    string updatedJsonData = JsonSerializer.Serialize(pelaajat, options);
                    File.WriteAllText(filePath, updatedJsonData);
                    bool continueGame = await DisplayAlert("Peli p‰‰ttyi", "Tasapeli! Haluatko pelata uudestaan?", "Kyll‰", "Lopeta");
                    if (continueGame)
                    {
                        // Jatka peli‰
                        ResetGame();
                    }
                    else
                    {
                        // Lopeta peli ja menn‰‰n takaisin alkuruutuun
                        ResetGame();
                        await Navigation.PushAsync(new MainPage());
                    }

                }

                // Tarkistaa onko kyseess‰ tietokone vastustaja ja samalla tekee tietokonevastustajan toimivuuden.
                if (tietokoneVastustaja && !pelaaja1Vuoro)
                {
                    tietokoneSiirto();
                }
            }
        }
    }

    private bool CheckForWin(string player)
    {
        // Tarkista voittoehdot t‰‰ll‰
        string[] lines = {
        "012", "345", "678", // Vaakasuorat
        "036", "147", "258", // Pystysuorat
        "048", "246" // Vinottaiset
        };

        foreach (var line in lines)
        {
            char a = board[line[0] - '0']?[0] ?? ' ';
            char b = board[line[1] - '0']?[0] ?? ' ';
            char c = board[line[2] - '0']?[0] ?? ' ';

            if (a == player[0] && a == b && a == c)
            {
                return true;
            }
        }

        return false;
    }

    // Funktio joka aloittaa peli alusta ja uusii arvot.
    private void ResetGame()
    {
        foreach (var button in buttons)
        {
            button.Text = "";
        }

        for (int i = 0; i < board.Length; i++)
        {
            board[i] = null;
        }

        pelaaja1Vuoro = true;
        PelaajaVuoro.Text = $"Pelaajan {pelaaja1.Etunimi} {pelaaja1.Sukunimi} vuoro";
        pelienYhteiskesto = 0;
        pelinAlkuaika = DateTime.Now;
        pelikelloKaynnistetty = false;
    }
}