using System.Collections.ObjectModel;
using System.Text.Json;

namespace final_work;

public partial class Tilasto : ContentPage
{
    public ObservableCollection<Pelaaja> Pelaajat { get; set; }
    public Tilasto()
	{
		InitializeComponent();
        Pelaajat = new ObservableCollection<Pelaaja>();
        LoadPelaajat();
        BindingContext = this;
	}

    // N�ytt�� pelaajien tiedot json tiedostosta.
    private void LoadPelaajat()
    {
        // Tarkistaa json tiedoston sovelluksen sis�lle bin kansiosta.
        string projectDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Pelaajat.json";
        string filePath = System.IO.Path.Combine(projectDirectory, fileName);

        if (System.IO.File.Exists(filePath))
        {
            // Lukee json tiedostosta datan
            string jsonData = System.IO.File.ReadAllText(filePath);
            ObservableCollection<Pelaaja> pelaajat = JsonSerializer.Deserialize<ObservableCollection<Pelaaja>>(jsonData);
            Pelaajat = pelaajat;
            OnPropertyChanged(nameof(Pelaajat));
   
        }
    }
}