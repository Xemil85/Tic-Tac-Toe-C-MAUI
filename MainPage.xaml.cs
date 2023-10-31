namespace final_work;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();

        AloitaPeli.Clicked += AloitaPeli_Clicked;
        LopetaPeli.Clicked += LopetaPeli_Clicked;
	}

    private void LopetaPeli_Clicked(object sender, EventArgs e)
    {
        System.Environment.Exit(0);
    }

    private async void AloitaPeli_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ValitsePelaaja());
    }
}

