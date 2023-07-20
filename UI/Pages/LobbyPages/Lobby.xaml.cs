using CommunityToolkit.Maui.Views;
using LogicUnit;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Objects.Enums;
using UI.Pages.LobbyPages.Utils;
using Game = UI.Pages.LobbyPages.Utils.Game;

namespace UI.Pages.LobbyPages;

//[QueryProperty(nameof(PlayerType), QueryIDs.k_PlayerType)]
//[QueryProperty(nameof(RoomCode), QueryIDs.k_Code)]
//[QueryProperty(nameof(PlayerName), QueryIDs.k_Name)]
public partial class Lobby : ContentPage
{
    //public string PlayerType { get; set; }
    private string m_Code;
    private PlayerType m_PlayerType;
    private string m_PlayerName;

    private LogicManager m_LogicManager = new LogicManager();
    public Button ChooseGameButton;
    public Button InstructionsButton;
    private GameCard m_ChosenGameCard;
    private Label GameDetailsLabel = new Label();
    private Game m_ChosenGame;
    private bool m_IsPageinitialized = false;

    public Lobby()
    {
        InitializeComponent();
        //StatusLabel.Text = "Waiting for all players...";
        m_PlayerName = m_LogicManager.m_Player.Name;
        m_Code = m_LogicManager.m_Player.RoomCode;
        m_PlayerType = m_LogicManager.m_Player.PlayerType;


        //m_LogicManager.SetAddPlayersAction(AddPlayers);
        //m_LogicManager.SetPlayersToRemoveAction(RemovedByHost);
        //m_LogicManager.StartPlayersListRefresher();
        //m_LogicManager.SetChosenGameAction(ShowChosenGame);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (m_PlayerType == PlayerType.Host)
        {
            Image removeBtn1 = new Image();
            removeBtn1.Source = "remove_btn.PNG";
            removeBtn1.HorizontalOptions = LayoutOptions.CenterAndExpand;
            removeBtn1.VerticalOptions = LayoutOptions.CenterAndExpand;
            Image removeBtn2 = new Image();
            removeBtn2.Source = "remove_btn.PNG";
            removeBtn2.HorizontalOptions = LayoutOptions.CenterAndExpand;
            removeBtn2.VerticalOptions = LayoutOptions.CenterAndExpand;
            Image removeBtn3 = new Image();
            removeBtn3.Source = "remove_btn.PNG";
            removeBtn3.HorizontalOptions = LayoutOptions.CenterAndExpand;
            removeBtn3.VerticalOptions = LayoutOptions.CenterAndExpand;
            grid.Add(removeBtn1, 2, 2);
            grid.Add(removeBtn2, 2, 3);
            grid.Add(removeBtn3, 2, 4);

            Image pickGameBtn = new Image();
            pickGameBtn.Source = "pick_a_game_btn.PNG";
            pickGameBtn.HorizontalOptions = LayoutOptions.CenterAndExpand;
            pickGameBtn.VerticalOptions = LayoutOptions.StartAndExpand;
            grid.Add(pickGameBtn, 4, 3);
            grid.SetColumnSpan(pickGameBtn, 2);

            Image snakeImg = new Image();
            snakeImg.Source = "snake_img.PNG";
            snakeImg.HorizontalOptions = LayoutOptions.CenterAndExpand;
            snakeImg.VerticalOptions = LayoutOptions.CenterAndExpand;
            snakeImg.ZIndex = 2;
            grid.Add(snakeImg, 5, 1);
            grid.SetRowSpan(snakeImg, 2);

            Image instructionsBtn = new Image();
            instructionsBtn.Source = "instructions_btn.PNG";
            instructionsBtn.HorizontalOptions = LayoutOptions.CenterAndExpand;
            instructionsBtn.VerticalOptions = LayoutOptions.CenterAndExpand;
            grid.Add(instructionsBtn, 4, 2);
        }
    }
}

//    [Obsolete]
//    protected override void OnNavigatedTo(NavigatedToEventArgs args)
//    {
//        base.OnNavigatedTo(args);

//        CodeLabel.Text = m_Code;

//        //PlayerCard playerCard = new PlayerCard(RemovePlayer, m_PlayerName);
//        //PlayersComponent.Add(playerCard);

//        if (m_PlayerType == PlayerType.Host)
//        {
//            //playerCard.AddRemoveButton();

//            ChooseGameButton = new Button();
//            ChooseGameButton.Text = "Choose a Game";
//            ChooseGameButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
//            ChooseGameButton.VerticalOptions = LayoutOptions.CenterAndExpand;
//            ChooseGameButton.Clicked += OnChooseGameClicked;
//            ButtonsComponent.Add(ChooseGameButton);
//        }

//        //m_LogicManager.SetAddPlayersAction(AddPlayers);
//        //m_LogicManager.SetPlayersToRemoveAction(RemovedByHost);
//        //m_LogicManager.SetChosenGameAction(ShowChosenGame);
//        //m_LogicManager.StartUpdatesRefresher();

//        //m_IsPageinitialized = true;

//        //List<string> list = new List<string>
//        //{
//        //    "Name",
//        //    "Player1",
//        //    "Player2",
//        //    "Player3"
//        //};
//        //AddPlayers(list);
//    }

//    protected override void OnAppearing()
//    {
//        base.OnAppearing();
//        m_LogicManager.SetAddPlayersAction(AddPlayers);
//        m_LogicManager.SetPlayersToRemoveAction(RemovedByHost);
//        m_LogicManager.SetChosenGameAction(ShowChosenGame);
//        //m_LogicManager.StartUpdatesRefresher();

//        m_IsPageinitialized = true;
//    }

//    private void OnLeaveClicked(object sender, EventArgs e)
//    {
//        m_LogicManager.PlayerLeft();
//        goToMainPage();
//    }

//    private async void goToMainPage()
//    {
//        if (m_PlayerType == PlayerType.Host)
//        {
//            await Shell.Current.GoToAsync("../.."); // two pages back - goes to the main page
//        }
//        else
//        {
//            await Shell.Current.GoToAsync("../../.."); // three pages back - goes to the main page
//        }
//    }

//    [Obsolete]
//    private void OnChooseGameClicked(object sender, EventArgs e)
//    {
//        GamesPopUp gamesPopUp = new GamesPopUp(UpdateChosenGame);

//        //GameCard snakeCard = new GameCard("snake.png", "Snake");
//        //GameCard pacmanCard = new GameCard("pacman.png", "Pacman");

//        Game snakeGame = Utils.GameLibrary.GetSnakeGame();
//        Game pacmanGame = Utils.GameLibrary.GetPacmanGame();

//        GameCard snakeCard = new GameCard(snakeGame);
//        GameCard pacmanCard = new GameCard(pacmanGame);

//        gamesPopUp.AddGameToComponent(snakeCard);
//        gamesPopUp.AddGameToComponent(pacmanCard);

//        this.ShowPopup(gamesPopUp);
//    }

//    public async void RemovePlayerByHost(PlayerCard i_PlayerCard, string i_PlayerName)
//    {
//        eLoginErrors logicResponse = await m_LogicManager.RemovePlayerByHost(m_Code, i_PlayerName);
//        //PlayersComponent.Remove(i_PlayerCard);
//    }

//    public void PlayerClickedLeave(string i_PlayerName)
//    {
//        foreach (PlayerCard card in PlayersComponent.Children.ToList())
//        {
//            if (card.CheckIfName(i_PlayerName))
//            {
//                PlayersComponent.Remove(card);
//            }
//        }
//    }

//    //public void ChangeStatusLabelToReady()
//    //{
//    //    StatusLabel.Text = "All players connected!";
//    //}

//    [Obsolete]
//    public void ShowChosenGame(string i_ChosenGameName)
//    {
//        Application.Current.Dispatcher.Dispatch(() =>
//        {
//            ChosenGameComponent.Remove(m_ChosenGameCard);
//            ChosenGameComponent.Remove(GameDetailsLabel);
//        });

//        //m_ChosenGame = i_ChosenGame;
//        m_ChosenGame = Utils.GameLibrary.GetGameByName(i_ChosenGameName);
//        m_ChosenGameCard = new GameCard(m_ChosenGame);

//        Application.Current.Dispatcher.Dispatch(() =>
//        {
//            ChosenGameComponent.Add(m_ChosenGameCard);

//            editGameDetailsLabel();
//            createInstructionsButton();
//        });

//    }

//    public void UpdateChosenGame(Game i_ChosenGame)
//    {
//        m_LogicManager.UpdateChosenGame(i_ChosenGame.GetName(), m_Code);
//    }

//    private void editGameDetailsLabel()
//    {
//        FormattedString formattedString = new FormattedString();

//        formattedString.Spans.Add(
//            new Span { Text = "Game Details:" + Environment.NewLine, FontAttributes = FontAttributes.Bold });
//        formattedString.Spans.Add(
//            new Span { Text = m_ChosenGame.GetDetails() });

//        GameDetailsLabel.FormattedText = formattedString;
//        GameDetailsLabel.HorizontalTextAlignment = TextAlignment.Center;
//        ChosenGameComponent.Add(GameDetailsLabel);
//    }

//    [Obsolete]
//    private void createInstructionsButton()
//    {
//        if (!ButtonsComponent.Children.Contains(InstructionsButton))
//        {
//            InstructionsButton = new Button();
//            InstructionsButton.Text = "Game Instructions";
//            InstructionsButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
//            InstructionsButton.VerticalOptions = LayoutOptions.CenterAndExpand;
//            InstructionsButton.Clicked += OnInstructionsBtnClicked;

//            ButtonsComponent.Add(InstructionsButton);
//        }
//    }

//    public void OnInstructionsBtnClicked(object sender, EventArgs e)
//    {
//        InstructionsPopUp instructionsPopUp =
//            new InstructionsPopUp(m_ChosenGame.GetName(), m_ChosenGame.GetInstructions());

//        this.ShowPopup(instructionsPopUp);
//    }


//    public bool AddPlayers(List<string> i_PlayersNames)
//    {
//        if (m_IsPageinitialized)
//        {
//            //PlayerCard newCard;

//            Application.Current.Dispatcher.Dispatch(PlayersComponent.Children.Clear);

//            Application.Current.Dispatcher.Dispatch(() =>
//            {
//                PlayersComponent.Add(new PlayerCard(RemovePlayerByHost, $"{m_PlayerName} (You)")); //adds the player itself
//            });

//            foreach (string name in i_PlayersNames)
//            {
//                if (name != m_PlayerName)
//                {
//                    if (m_PlayerType == PlayerType.Host)
//                    {
//                        //PlayerCard newCard = new PlayerCard(RemovePlayerByHost, name);
//                        //newCard.AddRemoveButton();
//                        Application.Current.Dispatcher.Dispatch(() =>
//                        {
//                            PlayersComponent.Add(new PlayerCard(RemovePlayerByHost, name, true));
//                        });
//                    }
//                    else
//                    {
//                        Application.Current.Dispatcher.Dispatch(() =>
//                        {
//                            PlayersComponent.Add(new PlayerCard(RemovePlayerByHost, name));
//                        });
//                    }

//                    //MainThread.BeginInvokeOnMainThread(() => PlayersComponent.Add(newCard));
//                    //Application.Current.Dispatcher.Dispatch(async () => PlayersComponent.Add(newCard));
//                }
//            }

//            return true;
//        }

//        return true;
//    }

//    public void RemovedByHost(List<string> i_RemovedPlayers)
//    {
//        if (i_RemovedPlayers.Contains(m_PlayerName))
//        {
//            // show a popup message with ok button
//            // when clicked OK move to the main page

//            RemovedByHostPopUp removedByHostPopUp = new RemovedByHostPopUp(goToMainPage);
//            Application.Current.Dispatcher.Dispatch(() =>
//            {
//                this.ShowPopup(removedByHostPopUp);
//            });
//        }
//    }



//    public async void Oncontinue(object sender, EventArgs e)
//    {
//        await Shell.Current.GoToAsync(nameof(ScreenPlacementSelectingPage));
//    }
//}