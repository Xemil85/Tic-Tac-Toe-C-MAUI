using System.Collections.ObjectModel;
using System.Text.Json;

namespace final_work;

public partial class ToinenPelaaja : ContentPage
{
    public ObservableCollection<Pelaaja> Pelaajat { get; set; }
    private Pelaaja pelaaja;

    [Obsolete]
    public ToinenPelaaja(Pelaaja pelaaja)
	{
		InitializeComponent();
        this.pelaaja = pelaaja;
        Pelaajat = new ObservableCollection<Pelaaja>();
        LoadPelaajat();
        BindingContext = this;
        ValitseToinenPelaaja.Clicked += ValitseToinenPelaaja_Clicked;
    }

    // Funktio jossa menn‰‰n valitaan listviewist‰ pelaaja ja sitten painetaan valitse pelaaja nappia, joka hypp‰‰ ristinolla n‰kym‰‰n.
    [Obsolete]
    private async void ValitseToinenPelaaja_Clicked(object sender, EventArgs e)
    {
        var selectedPelaaja = (Pelaaja)PelaajatListView.SelectedItem;

        if (selectedPelaaja != null)
        {
            Ristinolla ristinollaPage = new Ristinolla(pelaaja, selectedPelaaja, false);
            await Navigation.PushAsync(ristinollaPage);
        }
    }

    // Funktio joka etsii Pelaajat.json tiedostosta pelaajien datan ja huomioi ett‰ tietokone pelaajaa ei oteta mukaan siihen.
    private void LoadPelaajat()
    {
        string projectDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Pelaajat.json";
        string filePath = System.IO.Path.Combine(projectDirectory, fileName);

        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            ObservableCollection<Pelaaja> pelaajat = JsonSerializer.Deserialize<ObservableCollection<Pelaaja>>(jsonData);
            pelaajat = new ObservableCollection<Pelaaja>(pelaajat.Where(p => (p.Etunimi != "Tietokone") && (p.Etunimi != pelaaja.Etunimi)));
            Pelaajat = pelaajat;
        }
    }
}