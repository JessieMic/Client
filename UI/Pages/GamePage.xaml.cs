using System.Globalization;
using Microsoft.Maui.Controls.Shapes;
using GradientStop = Microsoft.Maui.Controls.GradientStop;
using LogicUnit;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
using Image = Objects.Image;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using UI.Pages.LobbyPages;
using CommunityToolkit.Maui.Views;
using LogicUnit.Logic.GamePageLogic;

namespace UI.Pages;

public partial class GamePage : ContentPage
{

    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private readonly InGameConnectionManager r_InGameConnectionManager = new();
    private Game m_Game;
    private Dictionary<int, Image> m_GameImages = new Dictionary<int, Image>();
    private Dictionary<int, ButtonImage> m_GameButtonsImages = new Dictionary<int, ButtonImage>();
    private Label m_GameLabel = new Label();

    public GamePage()
    {
        InitializeComponent();
        initializePage();
    }

    private void initializePage()
    {
        try
        {
            m_Game = m_GameLibrary.CreateAGame(m_GameInformation.NameOfGame, r_InGameConnectionManager);//m_GameInformation.m_NameOfGame);
            initializeEvents();
            initializeGame();
            runGame();
        }
        catch
        {
            serverError("sd");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    private void initializeGame()
    {
        addLabel(m_Game.InitializeGame());
    }

    public void addGameObjects(object sender, List<GameObject> i_GameObjectsToAdd)
    {
        lock (i_GameObjectsToAdd)
        {
            Application.Current.Dispatcher.Dispatch(async () =>
            {
                foreach (var gameObject in i_GameObjectsToAdd)
                {
                    if (gameObject.ScreenObjectType == eScreenObjectType.Button)
                    {
                        addButton(gameObject);
                    }
                    else// if (screenObject.ScreenObjectType == eScreenObjectType.Image)
                    {
                        addImage(gameObject);
                    }
                }
            });
        }
    }

    private void addLabel(GameObject i_Label)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            m_GameLabel.IsVisible = false;
            m_GameLabel.Rotation = i_Label.Rotatation;
            m_GameLabel.WidthRequest = i_Label.Size.Width;
            m_GameLabel.HeightRequest = i_Label.Size.Height;
            m_GameLabel.ZIndex = 1;
            m_GameLabel.TranslationX = i_Label.PointOnScreen.Column;
            m_GameLabel.TranslationY = i_Label.PointOnScreen.Row;
            m_GameLabel.FontAutoScalingEnabled = true;
            m_GameLabel.VerticalTextAlignment = TextAlignment.Center;
            m_GameLabel.HorizontalTextAlignment = TextAlignment.Center;
            gridLayout.Add(m_GameLabel);
        });
    }

    private void addImage(GameObject i_GameObjectToAdd)
    {
        Image image = new Image();
        image.SetImage(i_GameObjectToAdd);
        gridLayout.Add(image.GetImage());
        m_GameImages.Add(i_GameObjectToAdd.ID, image);
    }

    private void addButton(GameObject i_ButtonToAdd)
    {
        ButtonImage buttonImage = new ButtonImage();
        buttonImage.SetButtonImage(i_ButtonToAdd);
        buttonImage.ZIndex = 1;
        gridLayout.Add(buttonImage.GetImage());
        gridLayout.Add(buttonImage.GetButton());
        buttonImage.GetButton().Pressed += m_Game.OnButtonClicked;
        if (m_Game.DoesGameNeedToKnowIfButtonReleased() && i_ButtonToAdd.ButtonType != eButton.ButtonA)
        {
            buttonImage.GetButton().Released += m_Game.OnButtonRelesed;
        }

        m_GameButtonsImages.Add(i_ButtonToAdd.ID, buttonImage);
    }

    private void gameObjectUpdate(object sender, GameObject i_ObjectUpdate)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            if (getObjectTypeFromID(i_ObjectUpdate.ID) == eScreenObjectType.Image || i_ObjectUpdate.ScreenObjectType == eScreenObjectType.Player)
            {
                if (m_GameImages.ContainsKey(i_ObjectUpdate.ID))
                {
                    m_GameImages[i_ObjectUpdate.ID].Update(i_ObjectUpdate);
                }
            }
            else if (i_ObjectUpdate.ScreenObjectType == eScreenObjectType.Button)
            {
                if (m_GameButtonsImages.ContainsKey(i_ObjectUpdate.ID))
                {
                    m_GameButtonsImages[i_ObjectUpdate.ID].SetButtonImage(i_ObjectUpdate);
                }
            }
            if (i_ObjectUpdate.ScreenObjectType == eScreenObjectType.Label)
            {
                m_GameLabel.IsVisible = true;
                m_GameLabel.Text = i_ObjectUpdate.Text;
            }
        });
    }

    private eScreenObjectType getObjectTypeFromID(int ID)
    {
        eScreenObjectType type = eScreenObjectType.Image;

        if (m_GameButtonsImages.ContainsKey(ID))
        {
            type = eScreenObjectType.Button;
        }
        return type;
    }

    void runGame()
    {
        try
        {
            m_Game.RunGame();
        }
        catch (Exception e)
        {
            serverError($"{e.Message}{Environment.NewLine}error on m_Game.RunGame() in function runGame");
        }
    }

    async void exitGame()
    {
        clearGame();
        LogicManager logicManager = new LogicManager();
        logicManager.ResetRoomData();
        Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync(nameof(Lobby)));
    }

    void restartGame()
    {
        clearGame();
        initializePage();
    }

    void clearGame()
    {
        Application.Current.Dispatcher.Dispatch(async () =>
            {
                //disposeEvents();
                foreach (var button in m_GameButtonsImages)
                {
                    button.Value.Source = null;
                    gridLayout.Remove(button.Value.GetButton());
                }
                m_GameButtonsImages.Clear();
                foreach (var image in m_GameImages)
                {
                    image.Value.IsVisible = false;
                    gridLayout.Remove(image.Value.GetImage());
                }

                m_GameLabel.IsVisible = false;
                gridLayout.Remove(m_GameLabel);
                m_GameImages.Clear();
            });
    }

    void serverError(string i_Message)
    {
        this.Dispatcher.Dispatch(async () =>
        {
            await Shell.Current.GoToAsync("///MainPage");
            //MessagePopUp messagePopUp = new MessagePopUp(goToLobby, i_Message);
            //this.ShowPopup(messagePopUp);
            //Shell.Current.GoToAsync($"{nameof(Lobby)}?Error=True");
            //Shell.Current.GoToAsync("///MainPage");
        });
    }

    void goToLobby()
    {
        Shell.Current.GoToAsync(nameof(Lobby));
    }

    void initializeEvents()
    {
        try
        {
            m_Game.AddGameObjectList += addGameObjects;
            m_Game.GameObjectUpdate += gameObjectUpdate;
            m_Game.GameStart += runGame;
            m_Game.GameExit += exitGame;
            m_Game.GameRestart += restartGame;

            m_Game.ServerError += serverError;
            m_Game.DisposeEvents += disposeEvents;
        }
        catch
        {
            throw;
        }
    }

    void disposeEvents()
    {
        //m_Game.AddGameObjectList -= addGameObjects;
        //m_Game.GameObjectUpdate -= gameObjectUpdate;
        //m_Game.GameStart -= runGame;
        //m_Game.GameExit -= exitGame;
        //m_Game.GameRestart -= restartGame;
    }
}