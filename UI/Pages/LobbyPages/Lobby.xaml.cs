using CommunityToolkit.Maui.Views;
using LogicUnit;
using Objects;
using Objects.Enums;
using Game = UI.Pages.LobbyPages.Utils.Game;

namespace UI.Pages.LobbyPages;

public partial class Lobby : ContentPage
{
    private string m_Code;
    private PlayerType m_PlayerType;
    private string m_PlayerName;
    private LogicManager m_LogicManager = new LogicManager();
    private GameCard m_ChosenGameCard;
    private Label GameDetailsLabel = new Label();
    private Game m_ChosenGame;
    private bool m_IsPageinitialized = false;
    private List<Label> m_PlayersNamesLabels = new List<Label>(4);
    private List<ButtonImage> m_RemoveButtons = new List<ButtonImage>();
    private bool m_IsGameChosen = false;
    private Microsoft.Maui.Controls.Image m_ChosenGameImg;
    private ButtonImage m_InstructionsBtn = new ButtonImage();
    private GameInformation m_GameInformation = GameInformation.Instance;

    public Lobby()
    {
        InitializeComponent();
        //m_LogicManager.ResetRoomData();
        m_GameInformation.Reset();
        //StatusLabel.Text = "Waiting for all players...";
        m_PlayerName = m_GameInformation.Player.Name;
        m_Code = m_GameInformation.Player.RoomCode;
        m_PlayerType = m_GameInformation.Player.PlayerType;


        //m_LogicManager.SetAddPlayersAction(AddPlayers);
        //m_LogicManager.SetPlayersToRemoveAction(RemovedByHost);
        //m_LogicManager.StartPlayersListRefresher();
        //m_LogicManager.SetChosenGameAction(ShowChosenGame);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        //m_LogicManager.ResetRoomData();
        designLabelsText();

        CodeLabel.Text = m_Code;
        addImageButton("", OnLeaveClicked, 0, 0, "leave_btn.PNG",
            LayoutOptions.CenterAndExpand, LayoutOptions.StartAndExpand);
        m_PlayersNamesLabels[0].Text = m_PlayerName;

        if (m_PlayerType == PlayerType.Host)
        {
            ButtonImage pickGameBtn = addImageButton("Pick a Game", OnChooseGameClicked, 3, 4, "pick_a_game_btn.PNG",
                LayoutOptions.CenterAndExpand, LayoutOptions.CenterAndExpand);
            addImageButton("Start", OnStartClicked, 5, 5, "lobby_ready_btn.PNG",
            LayoutOptions.CenterAndExpand, LayoutOptions.CenterAndExpand);
            gridLayout.SetColumnSpan(pickGameBtn.GetImage(), 2);
            gridLayout.SetColumnSpan(pickGameBtn.GetButton(), 2);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //m_LogicManager.ResetRoomData();

        m_PlayersNamesLabels = new List<Label>()
        {
            Player1NameLabel,
            Player2NameLabel,
            Player3NameLabel,
            Player4NameLabel
        };

        m_LogicManager.SetAddPlayersAction(AddPlayers);
        m_LogicManager.SetPlayersToRemoveAction(RemovedByHost);
        m_LogicManager.SetChosenGameAction(ShowChosenGame);
        m_LogicManager.SetHostLeftAction(hostLeft);
        m_LogicManager.SetServerErrorAction(handleServerError);
        m_LogicManager.SetGoToNextPageAction(goToNextPage);
        m_LogicManager.StartUpdatesRefresher();

        m_IsPageinitialized = true;

        //Player2NameLabel.Text = "Paul";
        //Player3NameLabel.Text = "George";
        //Player4NameLabel.Text = "Ringo";
        //AddPlayers(new List<string>() { "Paul", "George", "Ringo" });
        //ShowChosenGame("Snake");
    }

    private ButtonImage addImageButton(string i_Text, EventHandler i_Event, int i_Row, int i_Col, string i_Source,
        LayoutOptions i_Vertical, LayoutOptions i_Horizontal)
    {
        ButtonImage btn = new ButtonImage();
        btn.Text = i_Text;
        btn.Source = i_Source;
        btn.GetButton().Clicked += i_Event;
        btn.VerticalOptions = i_Vertical;
        btn.HorizontalOptions = i_Horizontal;
        btn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
        btn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;

        gridLayout.Add(btn.GetImage(), i_Col, i_Row);
        gridLayout.Add(btn.GetButton(), i_Col, i_Row);

        return btn;
    }

    [Obsolete]
    private void OnChooseGameClicked(object sender, EventArgs e)
    {
        GamesPopUp gamesPopUp = new GamesPopUp(UpdateChosenGame);
        Game pacmanGame = Utils.GameLibrary.GetPacmanGame();
        Game bombItGame = Utils.GameLibrary.GetBombItGame();
        Game pongGame = Utils.GameLibrary.GetPongGame();
        GameCard pacmanCard = new GameCard(pacmanGame);
        GameCard bombItCard = new GameCard(bombItGame);
        GameCard pongCard = new GameCard(pongGame);

        gamesPopUp.AddGameToComponent(pacmanCard);
        gamesPopUp.AddGameToComponent(bombItCard);
        gamesPopUp.AddGameToComponent(pongCard);

        this.ShowPopup(gamesPopUp);
    }

    public void UpdateChosenGame(Game i_ChosenGame)
    {
        m_LogicManager.UpdateChosenGame(i_ChosenGame.GetName(), m_Code);
    }

    public void OnStartClicked(object sender, EventArgs e)
    {
        int amountOfPlayers = m_LogicManager.GetAmountOfPlayers();

        if (m_IsGameChosen)
        {
            if (amountOfPlayers == 1)
            {
                MessagePopUp messagePopUp = new MessagePopUp(Utils.Messages.k_WaitForPlayers);
                this.ShowPopup(messagePopUp);
            }
            else
            {
                if (m_ChosenGame == Utils.GameLibrary.GetPongGame() && amountOfPlayers == 3)
                {
                    MessagePopUp messagePopUp = new MessagePopUp(Utils.Messages.k_PongThreePlayers);
                    this.ShowPopup(messagePopUp);
                }
                else
                {
                    YesNoPopUp yesNoPopUp = new YesNoPopUp($"There are {amountOfPlayers} players in the room." +
                        $"{Environment.NewLine}Are you sure you want to continue?", updateServerToMoveToNextPage);
                    this.ShowPopup(yesNoPopUp);
                }
            }
        }
        else
        {
            MessagePopUp messagePopUp = new MessagePopUp(Utils.Messages.k_MustChooseGame);
            this.ShowPopup(messagePopUp);
        }
    }

    private void updateServerToMoveToNextPage()
    {
        m_LogicManager.UpdateServerToMoveToNextPage();
    }

    private void goToNextPage()
    {
        m_LogicManager.StopUpdatesRefresher();

        Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync(nameof(ScreenPlacementSelectingPage)));
    }

    private void OnLeaveClicked(object sender, EventArgs e)
    {
        m_LogicManager.StopUpdatesRefresher();
        if (m_PlayerType == PlayerType.Host)
        {
            m_LogicManager.HostLeft();
        }
        else
        {
            m_LogicManager.PlayerLeft();
        }
        
        goToMainPage();
    }

    private async void goToMainPage()
    {
        //if (m_PlayerType == PlayerType.Host)
        //{
        //    Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync("../..")); // two pages back - goes to the main page
        //}
        //else
        //{
        //    Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync("../../..")); // three pages back - goes to the main page
        //}

        await Shell.Current.GoToAsync("///MainPage");
    }

    public bool AddPlayers(List<string> i_PlayersNames)
    {
        int ind = 1;

        if (m_IsPageinitialized)
        {
            if (!i_PlayersNames.Contains(m_PlayerName))
            {
                removePlayerFromRoom();
                return true;
            }

            Application.Current.Dispatcher.Dispatch(() =>
            {
                for (int i = 1; i < m_PlayersNamesLabels.Count; i++)
                {
                    m_PlayersNamesLabels[i].Text = "";
                }

                foreach (ButtonImage removeBtn in m_RemoveButtons)
                {
                    gridLayout.Remove(removeBtn.GetImage());
                    gridLayout.Remove(removeBtn.GetButton());
                }

                m_RemoveButtons = new List<ButtonImage>();
            });

            //m_RemoveButtons = new List<ButtonImage>();

            foreach (string name in i_PlayersNames)
            {
                if (name != m_PlayerName)
                {
                    if (m_PlayerType == PlayerType.Host)
                    {
                        ButtonImage removeBtn = new ButtonImage();
                        removeBtn.Source = "remove_btn.PNG";
                        removeBtn.HorizontalOptions = LayoutOptions.CenterAndExpand;
                        removeBtn.VerticalOptions = LayoutOptions.CenterAndExpand;
                        removeBtn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;
                        removeBtn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
                        PlayerObjectController playerObj =
                                new PlayerObjectController(removeBtn, name, RemovePlayerByHost);
                        Application.Current.Dispatcher.Dispatch(() =>
                        {
                            m_RemoveButtons.Add(removeBtn);
                            gridLayout.Add(removeBtn.GetImage(), 2, ind + 1);
                            gridLayout.Add(removeBtn.GetButton(), 2, ind + 1);
                        });
                    }

                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        m_PlayersNamesLabels[ind].Text = name;
                        ind++;
                    });
                }
            }

            return true;
        }

        return false;
    }

    public async void RemovePlayerByHost(string i_PlayerName, ButtonImage i_RemoveButton)
    {
        eLoginErrors logicResponse = await m_LogicManager.RemovePlayerByHost(m_Code, i_PlayerName);

        if (logicResponse == eLoginErrors.ServerError)
        {
            handleServerError();
        }
    }

    public bool RemovedByHost(List<string> i_RemovedPlayers)
    {
        if (i_RemovedPlayers.Contains(m_PlayerName))
        {
            // show a popup message with ok button
            // when clicked OK move to the main page
            m_LogicManager.StopUpdatesRefresher();
            MessagePopUp removedByHostPopUp = new MessagePopUp(goToMainPage, Utils.Messages.k_RemovedByHost);
            Application.Current.Dispatcher.Dispatch(() =>
            {
                this.ShowPopup(removedByHostPopUp);
            });

            return true;
        }

        return false;
    }

    public void removePlayerFromRoom()
    {
        m_LogicManager.StopUpdatesRefresher();
        MessagePopUp removedByHostPopUp = new MessagePopUp(goToMainPage, Utils.Messages.k_RemovedByHost);
        Application.Current.Dispatcher.Dispatch(() =>
        {
            this.ShowPopup(removedByHostPopUp);
        });
    }

    private void hostLeft()
    {
        if (m_PlayerType == PlayerType.Guest)
        {
            m_LogicManager.StopUpdatesRefresher();
            MessagePopUp hostLeftPopUp = new MessagePopUp(goToMainPage, Utils.Messages.k_HostLeft);
            Application.Current.Dispatcher.Dispatch(() =>
            {
                this.ShowPopup(hostLeftPopUp);
            });
        }
    }

    private void handleServerError()
    {
        m_LogicManager.StopUpdatesRefresher();
        MessagePopUp messagePopUp = new MessagePopUp(goToMainPage, EnumHelper.GetDescription(eLoginErrors.ServerError));
        Application.Current.Dispatcher.Dispatch(() =>
        {
            this.ShowPopup(messagePopUp);
            return;
        });
    }

    [Obsolete]
    public void ShowChosenGame(string i_ChosenGameName)
    {
        Application.Current.Dispatcher.Dispatch(() =>
        {
            //ChosenGameComponent.Remove(m_ChosenGameCard);
            //ChosenGameComponent.Remove(GameDetailsLabel);
            if (m_IsGameChosen)
                gridLayout.Remove(m_ChosenGameImg);
        });

        //m_ChosenGame = i_ChosenGame;
        m_ChosenGame = Utils.GameLibrary.GetGameByName(i_ChosenGameName);
        m_ChosenGameCard = new GameCard(m_ChosenGame);

        Application.Current.Dispatcher.Dispatch(() =>
        {
            //ChosenGameComponent.Add(m_ChosenGameCard);

            //editGameDetailsLabel();
            createInstructionsButton();
            Microsoft.Maui.Controls.Image gameImg = new Microsoft.Maui.Controls.Image();
            gameImg.Source = m_ChosenGame.GetPicUrl();
            gameImg.HorizontalOptions = LayoutOptions.CenterAndExpand;
            gameImg.VerticalOptions = LayoutOptions.CenterAndExpand;
            gameImg.ZIndex = 2;
            Application.Current.Dispatcher.Dispatch(() =>
            {
                gridLayout.Add(gameImg, 5, 1);
                gridLayout.SetRowSpan(gameImg, 2);
            });
            m_ChosenGameImg = gameImg;
            m_IsGameChosen = true;
        });

    }

    [Obsolete]
    private void createInstructionsButton()
    {
        if (!gridLayout.Children.Contains(m_InstructionsBtn.GetButton()))
        {
            m_InstructionsBtn.Source = "instructions_btn.PNG";
            m_InstructionsBtn.HorizontalOptions = LayoutOptions.CenterAndExpand;
            m_InstructionsBtn.VerticalOptions = LayoutOptions.CenterAndExpand;
            m_InstructionsBtn.GetButton().Clicked += OnInstructionsBtnClicked;
            Application.Current.Dispatcher.Dispatch(() =>
            {
                gridLayout.Add(m_InstructionsBtn.GetButton(), 4, 2);
                gridLayout.Add(m_InstructionsBtn.GetImage(), 4, 2);
            });
        }
    }

    public void OnInstructionsBtnClicked(object sender, EventArgs e)
    {
        InstructionsPopUp instructionsPopUp =
            new InstructionsPopUp(m_ChosenGame.GetName(), m_ChosenGame.GetInstructions());

        this.ShowPopup(instructionsPopUp);
    }

    private void designLabelsText()
    {
        Color textColor = null;

        if (App.Current.Resources.TryGetValue("PaloozaTextColor", out var colorvalue))
            textColor = (Color)colorvalue;

        if (textColor != null)
        {
            CodeLabel.TextColor = textColor;
            CodeLabel.FontSize *= 1.15;

            foreach (Label label in m_PlayersNamesLabels)
            {
                label.TextColor = textColor;
                label.FontSize *= 1.15;
            }
        }
    }
}