using Microsoft.Maui.Controls.Shapes;
using GradientStop = Microsoft.Maui.Controls.GradientStop;
using LogicUnit;
using LogicUnit.Logic.GamePageLogic.Games.Snake;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
using Image = Objects.Image;
namespace UI.Pages;

public partial class GamePage : ContentPage
{

    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private Game m_Game;
    private Dictionary<int, Image> m_GameImages = new Dictionary<int, Image>();
    private Dictionary<int, ButtonImage> m_GameButtonsImages = new Dictionary<int, ButtonImage>();

    public GamePage()
    {
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

    private void gameObjectUpdate(object sender, GameObject i_ObjectUpdates)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            loopLabel.Text = m_Game.m_LoopNumber.ToString();
            //foreach (GameObject screenObject in m_Game.m_PlayerObjects)
            //{
                if (getObjectTypeFromID(i_ObjectUpdates.ID) == eScreenObjectType.Image)
                {
                    if (m_GameImages.ContainsKey(i_ObjectUpdates.ID))
                    {
                        m_GameImages[i_ObjectUpdates.ID].Update(i_ObjectUpdates);
                    }
                }
            //}
            //foreach (GameObject screenObject in m_Game.m_PlayerObjects)
            //{ 
            //    if (getObjectTypeFromID(screenObject.ID) == eScreenObjectType.Image)
            //    {
            //        if (m_GameImages.ContainsKey(screenObject.ID))
            //        {
            //            m_GameImages[screenObject.ID].Update(screenObject);
            //        }
            //    }
            //}
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

    private void hideGameObjects(object sender, List<int> i_IDlist)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var ID in i_IDlist)
            {
                if (getObjectTypeFromID(ID) == eScreenObjectType.Image)
                {
                    m_GameImages[ID].IsVisible = false;
                }
                else
                {
                    m_GameButtonsImages[ID].IsVisible = false;
                    m_GameButtonsImages[ID].IsEnabled = false;
                }
            }

            if (m_Game.m_GameStatus != eGameStatus.Restarted || m_Game.m_GameStatus != eGameStatus.Ended)
            {
                //m_GameImages.Clear();
                //m_gameButtons.Clear();
                //startGame();
            }
        });
    }

    private void showGameObjects(object sender, List<int> i_IDlist)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var ID in i_IDlist)
            {
                if (getObjectTypeFromID(ID) == eScreenObjectType.Image)
                {
                    m_GameImages[ID].IsVisible = true;
                    m_GameImages[ID].ZIndex = 0;
                }
                else
                {
                    m_GameButtonsImages[ID].IsVisible = true;
                    m_GameButtonsImages[ID].IsEnabled = true;
                    m_GameButtonsImages[ID].ZIndex = 1;
                }
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
        m_Game.RunGame();
    }

    void initializeEvents()
    {
        m_Game.AddGameObjectList += addGameObjects;
        m_Game.GameObjectUpdate += gameObjectUpdate;
        m_Game.GameObjectToDelete += deleteObject;
        m_Game.GameStart += runGame;
    }
}