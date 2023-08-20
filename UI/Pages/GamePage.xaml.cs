using System.Globalization;
using Microsoft.Maui.Controls.Shapes;
using GradientStop = Microsoft.Maui.Controls.GradientStop;
using LogicUnit;
using LogicUnit.Logic.GamePageLogic.Games.Snake;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
using Image = Objects.Image;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using UI.Pages.LobbyPages;

namespace UI.Pages;

public partial class GamePage : ContentPage
{

    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private Game m_Game;
    private Dictionary<int, Image> m_GameImages = new Dictionary<int, Image>();
    private Dictionary<int, ButtonImage> m_GameButtonsImages = new Dictionary<int, ButtonImage>();
    private Label m_GameLabel = new Label();
    private volatile static int g = 0;
    static readonly object m_lock = new object();
    private volatile static int firstThread = 0;

    public GamePage()
    {
        //lock (m_lock)
        //{
        //    if (g == 0)
        //    {
        //        Thread j = Thread.CurrentThread;
        //        if (m_GameInformation.Player.PlayerNumber == 1)
        //        {
        //            System.Diagnostics.Debug.WriteLine($"BLOCKED - Player num- {m_GameInformation.Player.PlayerNumber} Thread id-{j.ManagedThreadId} processor Id-{Thread.GetCurrentProcessorId()}");
        //        }
        //        g++;
        //        firstThread = j.ManagedThreadId;
        //    }
        //    else
        //    {
        //        Thread v = Thread.CurrentThread;
        //        g++;
                
        //        InitializeComponent();
        //        initializePage();
        //    }
        //}
        System.Diagnostics.Debug.WriteLine($"WEWEWWEWEW");
        InitializeComponent();
        initializePage();
    }
    private void initializePage()
    {
        m_Game = m_GameLibrary.CreateAGame(m_GameInformation.NameOfGame);//m_GameInformation.m_NameOfGame);
        initializeEvents();
        initializeGame();

    }

    private void initializeGame()
    {
        m_Game.InitializeGame();
    }

    public void addGameObjects(object sender, List<GameObject> i_GameObjectsToAdd)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var gameObject in i_GameObjectsToAdd)
            {
                if (gameObject.ScreenObjectType == eScreenObjectType.Label)
                {
                    addLabel(gameObject);
                }
                else if (gameObject.ScreenObjectType == eScreenObjectType.Button)
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

    private void addLabel(GameObject i_Label)
    {
        m_GameLabel.Text = i_Label.Text;
        m_GameLabel.Rotation = i_Label.Rotatation;
        m_GameLabel.WidthRequest = i_Label.Size.Width;
        m_GameLabel.HeightRequest = i_Label.Size.Height;
        m_GameLabel.ZIndex = i_Label.ZIndex;
        m_GameLabel.TranslationX = i_Label.PointOnScreen.Column;
        m_GameLabel.TranslationY = i_Label.PointOnScreen.Row;
        m_GameLabel.FontAutoScalingEnabled = true;
        m_GameLabel.VerticalTextAlignment = TextAlignment.Center;
        m_GameLabel.HorizontalTextAlignment = TextAlignment.Center;
        gridLayout.Add(m_GameLabel);
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
        buttonImage.GetButton().Clicked += m_Game.OnButtonClicked;
        m_GameButtonsImages.Add(i_ButtonToAdd.ID, buttonImage);
    }

    private void gameObjectUpdate(object sender, GameObject i_ObjectUpdate)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            //loopLabel.Text = m_Game.m_LoopNumber.ToString();
            if (getObjectTypeFromID(i_ObjectUpdate.ID) == eScreenObjectType.Image || i_ObjectUpdate.ScreenObjectType == eScreenObjectType.Player)
            {
                if (m_GameImages.ContainsKey(i_ObjectUpdate.ID))
                {
                    m_GameImages[i_ObjectUpdate.ID].Update(i_ObjectUpdate);
                }
            }
            else if(i_ObjectUpdate.ScreenObjectType == eScreenObjectType.Button)
            {
                if (m_GameButtonsImages.ContainsKey(i_ObjectUpdate.ID))
                {
                   m_GameButtonsImages[i_ObjectUpdate.ID].SetButtonImage(i_ObjectUpdate);
                }
            }
        });
    }
    

    public void deleteObject(object sender, GameObject? i_ObjectToDelete)
    {
        if (i_ObjectToDelete.Fade)
        {
            m_GameImages[i_ObjectToDelete.ID].FadeTo(0, 700);
        }
        else
        {
            m_GameImages[i_ObjectToDelete.ID].FadeTo(0, 100);
        }
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
        m_Game.RunGame();
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
            foreach (var button in m_GameButtonsImages)
            {
               button.Value.Source = null;
               gridLayout.Remove(button.Value.GetButton());
            }
            m_GameButtonsImages.Clear();
            foreach (var image in m_GameImages)
            {
                image.Value.Source = null;
                gridLayout.Remove(image.Value.GetImage());
            }
            gridLayout.Remove(m_GameLabel);
            m_GameImages.Clear();
        });
    }

    void initializeEvents()
    {
        m_Game.AddGameObjectList += addGameObjects;
        m_Game.GameObjectUpdate += gameObjectUpdate;
        m_Game.GameObjectToDelete += deleteObject;
        m_Game.GameStart += runGame;
        m_Game.GameExit += exitGame;
        m_Game.GameRestart += restartGame;
    }
}