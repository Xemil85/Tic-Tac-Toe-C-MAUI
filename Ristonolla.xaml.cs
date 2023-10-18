using System.Timers;

namespace final_work;

public partial class Ristinolla : ContentPage
{

    private Button[] buttons;
    private string[] board;
    private bool playerTurn;
    private bool gameEnded;
    private Pelaaja pelaaja1;
    private Pelaaja pelaaja2;

    public Ristinolla(Pelaaja pelaaja1, Pelaaja pelaaja2)
    {
        InitializeComponent();
        this.pelaaja1 = pelaaja1;
        this.pelaaja2 = pelaaja2;
        InitializeGame();

        UusiPeli.Clicked += UusiPeli_Clicked;

        Button0.Clicked += (sender, e) => OnButtonClick(0);
        Button1.Clicked += (sender, e) => OnButtonClick(1);
        Button2.Clicked += (sender, e) => OnButtonClick(2);
        Button3.Clicked += (sender, e) => OnButtonClick(3);
        Button4.Clicked += (sender, e) => OnButtonClick(4);
        Button5.Clicked += (sender, e) => OnButtonClick(5);
        Button6.Clicked += (sender, e) => OnButtonClick(6);
        Button7.Clicked += (sender, e) => OnButtonClick(7);
        Button8.Clicked += (sender, e) => OnButtonClick(8);
    }

    private void UusiPeli_Clicked(object sender, EventArgs e)
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        buttons = new Button[] { Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8 };
        board = new string[9];
        playerTurn = true;
        gameEnded = false;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Text = "";
            buttons[i].IsEnabled = true;
            int index = i; // Closure for event handler
            buttons[i].Clicked += (sender, e) => OnButtonClick(index);
        }

        PelaajaVuoro.Text = $"Pelaajan {pelaaja1.Etunimi} {pelaaja1.Sukunimi} vuoro";
    }

    private void OnButtonClick(int index)
    {
        if (gameEnded || board[index] != "")
            return;

        if (playerTurn)
        {
            buttons[index].Text = "X";
            board[index] = "X";
            PelaajaVuoro.Text = $"Pelaajan {pelaaja2.Etunimi} {pelaaja2.Sukunimi} vuoro";
        }
        else
        {
            buttons[index].Text = "O";
            board[index] = "O";
            PelaajaVuoro.Text = $"Pelaajan {pelaaja1.Etunimi} {pelaaja1.Sukunimi} vuoro";
        }

        buttons[index].IsEnabled = false;

        if (CheckForWin())
        {
            gameEnded = true;
            PelaajaVuoro.Text = (playerTurn ? $"Pelaaja {pelaaja1.Etunimi} {pelaaja1.Sukunimi}" : $"Pelaaja {pelaaja2.Etunimi} {pelaaja2.Sukunimi}") + " voitti!";
        }
        else if (board.All(cell => cell != ""))
        {
            gameEnded = true;
            PelaajaVuoro.Text = "Tasapeli!";
        }

        playerTurn = !playerTurn;
    }

    private bool CheckForWin()
    {
        string[] lines = {
            "012", "345", "678", // Vaakasuorat
            "036", "147", "258", // Pystysuorat
            "048", "246" // Vinottaiset
        };

        foreach (var line in lines)
        {
            char a = board[line[0] - '0'][0];
            char b = board[line[1] - '0'][0];
            char c = board[line[2] - '0'][0];

            if (a != '\0' && a == b && a == c)
            {
                return true;
            }
        }

        return false;
    }
}