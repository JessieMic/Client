//using Android.Widget;

using LogicUnit;
using LogicUnit.Logic.GamePageLogic.Games.Snake;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
namespace UI.Pages;

public partial class GamePage : ContentPage
{

    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private Game m_Game;
    private Dictionary<int,Image> m_GameImages = new Dictionary<int,Image>();
    private Dictionary<int,ImageButton> m_gameButtons = new Dictionary<int, ImageButton>();

    public GamePage()
    {
        InitializeComponent();
        //startGame();
        initializePage();
        m_Game.RunGame();
    }

    private void initializePage()
    {
        m_Game = m_GameLibrary.CreateAGame(eGames.Snake);//m_GameInformation.m_NameOfGame);
        initializeEvents();
        initializeGame();

    }

    private void initializeGame()
    {
        m_Game.InitializeGame();
    }

    public void addGameObjects(object sender, List<GameObject> i_GameObjectsToAdd)
    {
        int i = 1;
        foreach (var gameObject in i_GameObjectsToAdd)
        {
            if (gameObject.m_ScreenObjectType == eScreenObjectType.Button)
            {
                addButton(gameObject);
            }
            else// if (screenObject.m_ScreenObjectType == eScreenObjectType.Image)
            {
                addImage(gameObject, i);
                if (gameObject.m_ImageSources[0][5] == 'p' && gameObject.m_ImageSources[0][11] == '1')
                {
                    i++;
                }
            }
        }
    }

    private void addImage(GameObject i_GameObjectToAdd,int i)
    {
        Image image = new Image();

        image.TranslationX = i_GameObjectToAdd.m_PointsOnScreen[0].m_Column;//.m_Point.m_Column;
        image.TranslationY = i_GameObjectToAdd.m_PointsOnScreen[0].m_Row;

        if (i_GameObjectToAdd.m_OurSize.m_Width != 0)
        {
            image.WidthRequest = i_GameObjectToAdd.m_OurSize.m_Width;
        }

        if (i_GameObjectToAdd.m_OurSize.m_Height != 0)
        {
            image.HeightRequest = i_GameObjectToAdd.m_OurSize.m_Height;
        }

        if (i_GameObjectToAdd.m_ImageSources[0][5] == 'p')
        {
            image.ClassId = "snake" + i.ToString() + ".png";
            image.Source = "snake" + i.ToString() + ".png";
        }
        else
        {
            image.ClassId = i_GameObjectToAdd.m_ImageSources[0];
            image.Source = i_GameObjectToAdd.m_ImageSources[0];
        }
        image.ZIndex = -1;
        image.Rotation = i_GameObjectToAdd.m_rotate;
        image.Aspect = Aspect.AspectFill;
        
        gridLayout.Add(image);
        m_GameImages.Add(i_GameObjectToAdd.m_ID[0],image);
    }


    private void addButton(GameObject i_ButtonToAdd)
    {
        ImageButton button = new ImageButton();
        //Button button = new Button();
        button.ClassId = i_ButtonToAdd.m_ButtonType.ToString();//.m_KindOfButton.ToString();
        button.HeightRequest = i_ButtonToAdd.m_OurSize.m_Height;
        button.WidthRequest = i_ButtonToAdd.m_OurSize.m_Width;
        button.CornerRadius = 70;
        gridLayout.Add(button);
        button.TranslationX = i_ButtonToAdd.m_PointsOnScreen[0].m_Column;
        button.TranslationY = i_ButtonToAdd.m_PointsOnScreen[0].m_Row;
        
        //button.Text = i_ButtonToAdd.m_ButtonType.ToString();
        button.Rotation = i_ButtonToAdd.m_rotate;
        button.Source = i_ButtonToAdd.m_ImageSources[0];
        //button.Aspect = Aspect.AspectFit;
        button.ZIndex = -1;
        m_gameButtons.Add(i_ButtonToAdd.m_ID[0],button);
        button.Clicked += m_Game.OnButtonClicked;
    }

    private void gameObjectsUpdate(object sender, List<GameObject> i_ObjectUpdates)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var screenObject in i_ObjectUpdates)
            {
                for(int i = 0; i < screenObject.m_ID.Count; i++)
                {
                    if(getObjectTypeFromID(screenObject.m_ID[i]) == eScreenObjectType.Image)
                    {
                        if(m_GameImages.ContainsKey(screenObject.m_ID[i]))
                        {
                            m_GameImages[screenObject.m_ID[i]].TranslationX = screenObject.m_PointsOnScreen[i].m_Column;
                            m_GameImages[screenObject.m_ID[i]].TranslationY = screenObject.m_PointsOnScreen[i].m_Row;
                        }
                    }
                }
            }
        });
    }

    public void deleteObject(object sender, GameObject i_ObjectToDelete)
    {

        for (int i = 0; i < i_ObjectToDelete.m_ID.Count; i++)
        {
            if(i_ObjectToDelete.m_Fade)
            {
                m_GameImages[i_ObjectToDelete.m_ID[i]].FadeTo(0, 700, null);
            }
            else
            {
                m_GameImages[i_ObjectToDelete.m_ID[i]].FadeTo(0, 100, null);
            }
            //gridLayout.Remove(m_GameImages[i_ObjectToDelete.m_ID[i]]);
            //m_GameImages.Remove(i_ObjectToDelete.m_ID[i]);
        }
    }

    private void hideGameObjects(object sender, List<int> i_IDlist)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var ID in i_IDlist)
            {
                if(getObjectTypeFromID(ID) == eScreenObjectType.Image)
                {
                    m_GameImages[ID].IsVisible = false;
                }
                else
                {
                    m_gameButtons[ID].IsVisible = false;
                    m_gameButtons[ID].IsEnabled = false;
                }
            }

            if(m_Game.m_GameStatus != eGameStatus.Restarted || m_Game.m_GameStatus != eGameStatus.Ended)
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
                    m_gameButtons[ID].IsVisible = true;
                    m_gameButtons[ID].IsEnabled = true;
                    m_gameButtons[ID].ZIndex = 1;
                }
            }
        });
    }

    private eScreenObjectType getObjectTypeFromID(int ID)
    {
        eScreenObjectType type = eScreenObjectType.Image;

        if(m_gameButtons.ContainsKey(ID))
        {
            type = eScreenObjectType.Button;
        }
        return type;
    }

    void initializeEvents()
    {
        m_Game.AddGameObjectList += addGameObjects;
        m_Game.GameObjectsUpdate += gameObjectsUpdate;
        m_Game.GameObjectToDelete += deleteObject;
        m_Game.GameObjectsToHide +=hideGameObjects;
        m_Game.GameObjectsToShow += showGameObjects;
    }
}

//playerObject.Source = screenObject.m_ImageSources[i];
//playerObject.TranslateTo(
//    screenObject.m_NewPositions[i].m_Column,
//    screenObject.m_NewPositions[i].m_Row);