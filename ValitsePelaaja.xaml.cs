namespace final_work;

public partial class ValitsePelaaja : ContentPage
{
	public ValitsePelaaja()
	{
		InitializeComponent();

        UusiPelaaja.Clicked += UusiPelaaja_Clicked;
        EntinenPelaaja.Clicked += EntinenPelaaja_Clicked;
    }

    private async void EntinenPelaaja_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Entinenpelaaja());
    }

    private async void UusiPelaaja_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UusiPelaaja());
    }
}