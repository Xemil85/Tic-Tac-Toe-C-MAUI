using System.Diagnostics;
using System.Timers;

namespace final_work;

public partial class Ristinolla : ContentPage
{

    private bool pelaaja1Vuoro = true;
    private List<Button> buttons;
    private string[] board;
    private Pelaaja pelaaja1;
    private Pelaaja pelaaja2;

    
    public Ristinolla(Pelaaja pelaaja1, Pelaaja pelaaja2)
    {
        InitializeComponent();
        this.pelaaja1 = pelaaja1;
        this.pelaaja2 = pelaaja2;

        buttons = new List<Button> { Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8 };
        board = new string[9];

        foreach (var button in buttons)
        {
            button.Clicked += OnButtonClick;
        }

        PelaajaVuoro.Text = $"Pelaajan {pelaaja1.Etunimi} {pelaaja1.Sukunimi} vuoro";
    }

    private void OnButtonClick(object sender, EventArgs e)
    {
        var button = (Button)sender;
        int index = buttons.IndexOf(button);

        if (board[index] == null)
        {
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

            if (CheckForWin("X"))
            {
                DisplayAlert("Peli p‰‰ttyi", $"Pelaaja {pelaaja1.Etunimi} {pelaaja1.Sukunimi} voitti!", "OK");
                ResetGame();
            }
            else if (CheckForWin("O"))
            {
                DisplayAlert("Peli p‰‰ttyi", $"Pelaaja {pelaaja2.Etunimi} {pelaaja2.Sukunimi} voitti!", "OK");
                ResetGame();
            }
            else if (board.All(cell => cell != null))
            {
                DisplayAlert("Peli p‰‰ttyi", "Tasapeli!", "OK");
                ResetGame();
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
    }


}