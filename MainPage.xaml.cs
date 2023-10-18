namespace final_work;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();

        UusiPelaaja.Clicked += UusiPelaaja_Clicked;
        EntinenPelaaja.Clicked += EntinenPelaaja_Clicked;
        //Ristinolla.Clicked += Ristinolla_Clicked;
        //Vastustaja.Clicked += Vastustaja_Clicked;
	}

    //private async void Vastustaja_Clicked(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new Vastustaja());
    //}

    //private async void Ristinolla_Clicked(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new Ristinolla());
    //}

    private async void EntinenPelaaja_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Entinenpelaaja());
    }

    private async void UusiPelaaja_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UusiPelaaja());
    }
}

