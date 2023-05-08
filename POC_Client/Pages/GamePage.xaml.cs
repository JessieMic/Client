//using Android.Widget;

using LogicUnit;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
namespace POC_Client.Pages;

public partial class GamePage : ContentPage
{

    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private Game m_Game;
    private List<List<Image>> m_PlayerObjects = new List<List<Image>>();
    private List<Image> m_GameObjects = new List<Image>();

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
        //Application.Current.Dispatcher.Dispatch(async () =>
        //    {

        //        gridLayout.Add(m_Game.a);
        //    });
        m_Game.InitializeGame();
        for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
        {
            m_PlayerObjects.Add(new List<Image>());
        }
        
    }

    public void addGameObjects(object sender, List<GameObject> i_GameObjectsToAdd)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
            {
                //gridLayout.Add(m_Game.a);
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

        if (i_GameObjectToAdd.m_Size.m_Width != 0)
        {
            image.WidthRequest = i_GameObjectToAdd.m_Size.m_Width;
        }

        if (i_GameObjectToAdd.m_Size.m_Height != 0)
        {
            image.HeightRequest = i_GameObjectToAdd.m_Size.m_Height;
        }

        image.Aspect = Aspect.AspectFill;

        gridLayout.Add(image);
        if (i_GameObjectToAdd.m_ScreenObjectType != eScreenObjectType.Image)
        {
            addGameObjectToList(image, i_GameObjectToAdd);
        }
        image.Source = i_GameObjectToAdd.m_ImageSources[0];
    }

    private void addGameObjectToList(Image i_image, GameObject i_GameObject)
    {
        if (i_GameObject.m_ScreenObjectType == eScreenObjectType.Player)
        {
            m_PlayerObjects[i_GameObject.m_ObjectNumber - 1].Add(i_image);
        }
        else
        {
            m_GameObjects.Add(i_image);
        }
    }

    private void addButton(GameObject i_ButtonToAdd)
    {
        Button button = new Button();
        button.ClassId = i_ButtonToAdd.m_ButtonType.ToString();//.m_KindOfButton.ToString();
        button.HeightRequest = i_ButtonToAdd.m_Size.m_Height;
        button.WidthRequest = i_ButtonToAdd.m_Size.m_Width;
        button.CornerRadius = 70;
        gridLayout.Add(button);
        button.TranslateTo(i_ButtonToAdd.m_PointsOnScreen[0].m_Column, i_ButtonToAdd.m_PointsOnScreen[0].m_Row);
        button.Clicked += m_Game.OnButtonClicked;
    }

    private void gameObjectsUpdate(object sender, List<GameObject> i_ObjectUpdates)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
            {
                foreach (var screenObject in i_ObjectUpdates)
                {
                    int i = 0;
                    if (screenObject.m_ScreenObjectType == eScreenObjectType.Player)
                    {
                        foreach (var playerObject in m_PlayerObjects[screenObject.m_ObjectNumber - 1])
                        {
                            //playerObject.Source = screenObject.m_ImageSources[i];
                            //playerObject.TranslateTo(
                            //    screenObject.m_NewPositions[i].m_Column,
                            //    screenObject.m_NewPositions[i].m_Row);
                            playerObject.TranslationX = screenObject.m_PointsOnScreen[i].m_Column;//m_NewPositions[i].m_Column;
                            playerObject.TranslationY = screenObject.m_PointsOnScreen[i].m_Row;//.m_NewPositions[i].m_Row;
                            i++;
                        }
                    }
                    else
                    {
                        m_GameObjects[screenObject.m_ObjectNumber - 1].Source = screenObject.m_ImageSources[i];
                        m_GameObjects[screenObject.m_ObjectNumber - 1].TranslationX = screenObject.m_PointsOnScreen[i].m_Column;//.m_NewPositions[i].m_Column;
                        m_GameObjects[screenObject.m_ObjectNumber - 1].TranslationY = screenObject.m_PointsOnScreen[i].m_Row;
                        ;//.m_NewPositions[i].m_Row;
                    }
                }
            });
    }

    public void deleteObject(object sender, GameObject i_ObjectToDelete)
    {
        if(i_ObjectToDelete.m_ScreenObjectType == eScreenObjectType.Player)
        {
            foreach(var image in m_PlayerObjects[i_ObjectToDelete.m_ObjectNumber - 1])
            {
                image.FadeTo(0, 700, null);
                //gridLayout.Remove(image);
            }

            m_PlayerObjects[i_ObjectToDelete.m_ObjectNumber - 1].Clear();
        }
        else
        {
            gridLayout.Remove(m_GameObjects[i_ObjectToDelete.m_ObjectNumber - 1]);
        }
    }

    async Task initializeEvents()
    {
        m_Game.AddGameObjectList += addGameObjects;
        m_Game.GameObjectsUpdate += gameObjectsUpdate;
        m_Game.GameObjectToDelete += deleteObject;
    }
}