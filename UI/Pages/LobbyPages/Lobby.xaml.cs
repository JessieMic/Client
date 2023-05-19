using CommunityToolkit.Maui.Views;
using LogicUnit;
using UI.Pages.LobbyPages.Utils;

namespace UI.Pages.LobbyPages;

[QueryProperty(nameof(PlayerType), QueryIDs.k_PlayerType)]
[QueryProperty(nameof(RoomCode), QueryIDs.k_Code)]
[QueryProperty(nameof(PlayerName), QueryIDs.k_Name)]
public partial class Lobby : ContentPage
{
    public string PlayerType { get; set; }
    string m_code;
    private LogicManager m_LogicManager = new LogicManager();
    public Button ChooseGameButton;
    public Button InstructionsButton;
    private GameCard m_ChosenGameCard;
    private Label GameDetailsLabel = new Label();
    private Game m_ChosenGame;

    public string RoomCode {
        get => m_code;
        set
        {
            m_code = value;
            CodeLabel.Text = value;
        }
    }
    public string PlayerName { get; set; }

    public Lobby()
	{
        InitializeComponent();
        StatusLabel.Text = "Waiting for all players...";
	}

    [Obsolete]
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        PlayerCard playerCard = new PlayerCard(RemovePlayer, PlayerName);
        PlayersComponent.Add(playerCard);

        if (PlayerType == LogicUnit.PlayerType.k_Host)
        {
            //playerCard.AddRemoveButton();

            ChooseGameButton = new Button();
            ChooseGameButton.Text = "Choose a Game";
            ChooseGameButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ChooseGameButton.VerticalOptions = LayoutOptions.CenterAndExpand;
            ChooseGameButton.Clicked += OnChooseGameClicked;
            ButtonsComponent.Add(ChooseGameButton);
        }

        m_LogicManager.SetAddPlayersAction(AddPlayers);

        //List<string> list = new List<string>
        //{
        //    "Name",
        //    "Player1",
        //    "Player2",
        //    "Player3"
        //};
        //AddPlayers(list);
    }

    private async void OnLeaveClicked(object sender, EventArgs e)
    {
        m_LogicManager.PlayerLeft(RoomCode, PlayerName);

        if (PlayerType == LogicUnit.PlayerType.k_Host)
        {
            await Shell.Current.GoToAsync("../.."); // two pages back - goes to the main page
        }
        else
        {
            await Shell.Current.GoToAsync("../../..");
        }
    }

    [Obsolete]
    private void OnChooseGameClicked(object sender, EventArgs e)
    {
        GamesPopUp gamesPopUp = new GamesPopUp(ShowChosenGame);

        //GameCard snakeCard = new GameCard("snake.png", "Snake");
        //GameCard pacmanCard = new GameCard("pacman.png", "Pacman");

        Game snakeGame = GameLibrary.GetSnakeGame();
        Game pacmanGame = GameLibrary.GetPacmanGame();

        GameCard snakeCard = new GameCard(snakeGame);
        GameCard pacmanCard = new GameCard(pacmanGame);

        gamesPopUp.AddGameToComponent(snakeCard);
        gamesPopUp.AddGameToComponent(pacmanCard);

        this.ShowPopup(gamesPopUp);
    }

    public void RemovePlayer(PlayerCard i_PlayerCard, string i_PlayerName)
    {
        m_LogicManager.RemovePlayerByHost(RoomCode, i_PlayerName);
        PlayersComponent.Remove(i_PlayerCard);
    }

    public void DeletePlayerFromScreen(string i_PlayerName)
    {
        foreach(PlayerCard card in PlayersComponent.Children.ToList())
        {
            if(card.CheckIfName(i_PlayerName))
            {
                PlayersComponent.Remove(card);
            }
        }
    }

    public void ChangeStatusLabelToReady()
    {
        StatusLabel.Text = "All players connected!";
    }

    [Obsolete]
    public void ShowChosenGame(Game i_ChosenGame)
    {
        ChosenGameComponent.Remove(m_ChosenGameCard);
        ChosenGameComponent.Remove(GameDetailsLabel);

        m_ChosenGame = i_ChosenGame;
        m_ChosenGameCard = new GameCard(i_ChosenGame);
        ChosenGameComponent.Add(m_ChosenGameCard);

        editGameDetailsLabel();
        createInstructionsButton();
    }

    private void editGameDetailsLabel()
    {
        FormattedString formattedString = new FormattedString();

        formattedString.Spans.Add(
            new Span { Text = "Game Details:" + Environment.NewLine, FontAttributes = FontAttributes.Bold });
        formattedString.Spans.Add(
            new Span { Text = m_ChosenGame.GetDetails() });

        GameDetailsLabel.FormattedText = formattedString;
        GameDetailsLabel.HorizontalTextAlignment = TextAlignment.Center;
        ChosenGameComponent.Add(GameDetailsLabel);
    }

    [Obsolete]
    private void createInstructionsButton()
    {
        if (!ButtonsComponent.Children.Contains(InstructionsButton))
        {
            InstructionsButton = new Button();
            InstructionsButton.Text = "Game Instructions";
            InstructionsButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
            InstructionsButton.VerticalOptions = LayoutOptions.CenterAndExpand;
            InstructionsButton.Clicked += OnInstructionsBtnClicked;

            ButtonsComponent.Add(InstructionsButton);
        }
    }

    public void OnInstructionsBtnClicked(object sender, EventArgs e)
    {
        InstructionsPopUp instructionsPopUp =
            new InstructionsPopUp(m_ChosenGame.GetName(), m_ChosenGame.GetInstructions());

        this.ShowPopup(instructionsPopUp);
    }

    public void AddPlayers(List<string> i_PlayersNames)
    {
        foreach (string name in i_PlayersNames)
        {
            if (name != PlayerName)
            {
                PlayerCard newCard = new PlayerCard(RemovePlayer, name);

                if (PlayerType == LogicUnit.PlayerType.k_Host)
                {
                    newCard.AddRemoveButton();
                }

                PlayersComponent.Add(newCard);
            }
        }
    }
}