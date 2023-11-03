namespace final_work;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
        AloitaPeli.Clicked += AloitaPeli_Clicked;
        Tilasto.Clicked += Tilasto_Clicked;
        LopetaPeli.Clicked += LopetaPeli_Clicked;
	}

    // Funktio tilasto napin toimivuudelle jossa päästään tilastonäkymään.
    private async void Tilasto_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Tilasto());
    }

    // Lopettaa pelin eli sammuttaa sovelluksen kokonaan.
    private void LopetaPeli_Clicked(object sender, EventArgs e)
    {
        System.Environment.Exit(0);
    }

    // Funktio jossa mennään aloita peli ruutuun jossa on kaksi nappia.
    private async void AloitaPeli_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ValitsePelaaja());
    }
}

