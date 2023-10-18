using System.Collections.ObjectModel;
using System.Text.Json;

namespace final_work;

public partial class Entinenpelaaja : ContentPage
{
    public ObservableCollection<Pelaaja> Pelaajat { get; set; }

    public Entinenpelaaja()
    {
        InitializeComponent();
        Pelaajat = new ObservableCollection<Pelaaja>();
        LoadPelaajat();
        BindingContext = this;
        ValitsePelaaja.Clicked += ValitsePelaaja_Clicked;
    }

    private async void ValitsePelaaja_Clicked(object sender, EventArgs e)
    {
        var selectedPelaaja = (Pelaaja)PelaajatListView.SelectedItem;

        if (selectedPelaaja != null)
        {
            // Siirry takaisin Vastustaja-n‰kym‰‰n ja l‰het‰ pelaajan tiedot
            await Navigation.PushAsync(new Vastustaja(selectedPelaaja));
        }
    }

    private void LoadPelaajat()
    {
        string projectDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Pelaajat.json";
        string filePath = System.IO.Path.Combine(projectDirectory, fileName);

        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            ObservableCollection<Pelaaja> pelaajat = JsonSerializer.Deserialize<ObservableCollection<Pelaaja>>(jsonData);
            pelaajat = new ObservableCollection<Pelaaja>(pelaajat.Where(p => p.Etunimi != "Tietokone"));
            Pelaajat = pelaajat;
        }
    }
}