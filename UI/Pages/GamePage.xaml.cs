//using Android.Widget;

using LogicUnit;
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
    private Dictionary<int,Button> m_gameButtons = new Dictionary<int,Button>();

    public GamePage()
    {
        InitializeComponent();
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
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var gameObject in i_GameObjectsToAdd)
            {
                if (gameObject.m_ScreenObjectType == eScreenObjectType.Button)
                {
                    addButton(gameObject);
                }
                else// if (screenObject.m_ScreenObjectType == eScreenObjectType.Image)
                {
                    addImage(gameObject);
                }
            }
        });
    }

    private void addImage(GameObject i_GameObjectToAdd)
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

        image.Aspect = Aspect.AspectFill;
        image.ClassId = i_GameObjectToAdd.m_ImageSources[0];
        gridLayout.Add(image);
        m_GameImages.Add(i_GameObjectToAdd.m_ID[0],image);
        image.Source = i_GameObjectToAdd.m_ImageSources[0];
    }


    private void addButton(GameObject i_ButtonToAdd)
    {
        Button button = new Button();
        button.ClassId = i_ButtonToAdd.m_ButtonType.ToString();//.m_KindOfButton.ToString();
        button.HeightRequest = i_ButtonToAdd.m_OurSize.m_Height;
        button.WidthRequest = i_ButtonToAdd.m_OurSize.m_Width;
        button.CornerRadius = 70;
        gridLayout.Add(button);
        button.TranslationX = i_ButtonToAdd.m_PointsOnScreen[0].m_Column;
        button.TranslationY = i_ButtonToAdd.m_PointsOnScreen[0].m_Row;
        button.Clicked += m_Game.OnButtonClicked;
        m_gameButtons.Add(i_ButtonToAdd.m_ID[0],button);
    }

    private void gameObjectsUpdate(object sender, List<GameObject> i_ObjectUpdates)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var screenObject in i_ObjectUpdates)
            {
                for(int i = 0; i < screenObject.m_ID.Count; i++)
                {
                    m_GameImages[screenObject.m_ID[i]].TranslationX = screenObject.m_PointsOnScreen[i].m_Column;
                    m_GameImages[screenObject.m_ID[i]].TranslationY = screenObject.m_PointsOnScreen[i].m_Row;
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
            //gridLayout.Remove(m_GameImages[i_ObjectToDelete.m_ID[i]]);
            //m_GameImages.Remove(i_ObjectToDelete.m_ID[i]);
        }
    }

    async Task initializeEvents()
    {
        m_Game.AddGameObjectList += addGameObjects;
        m_Game.GameObjectsUpdate += gameObjectsUpdate;
        m_Game.GameObjectToDelete += deleteObject;
    }
}

//playerObject.Source = screenObject.m_ImageSources[i];
//playerObject.TranslateTo(
//    screenObject.m_NewPositions[i].m_Column,
//    screenObject.m_NewPositions[i].m_Row);