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

        m_Game = m_GameLibrary.CreateAGame(eGames.Snake);//m_GameInformation.m_NameOfGame);
        initializeEvents();
        m_Game.InitializeGame();
        for(int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
        {
            m_PlayerObjects.Add(new List<Image>());
        }
        m_Game.RunGame();
    }

    public void addScreenObject(object sender, List<ScreenObjectAdd> i_ScreenObject)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            
            foreach(var screenObject in i_ScreenObject)
            {
                if (screenObject.m_ScreenObjectType == eScreenObjectType.Button)
                {
                    addButton(screenObject);
                }
                else// if (screenObject.m_ScreenObjectType == eScreenObjectType.Image)
                {
                    addImage(screenObject);
                }
            }
        });
    }
    private void gameObjectUpdate(object sender , List<ScreenObjectUpdate> i_ObjectUpdates)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach (var screenObject in i_ObjectUpdates)
            {
                int i = 0;
                if (screenObject.m_ScreenObjectType == eScreenObjectType.PlayerObject)
                {
                    foreach (var playerObject in m_PlayerObjects[screenObject.m_ObjectNumber-1])
                    {
                        //playerObject.Source = screenObject.m_ImageSources[i];
                        playerObject.TranslateTo(
                            screenObject.m_NewPositions[i].m_Column,
                            screenObject.m_NewPositions[i].m_Row);
                        i++;
                    }
                }
                else
                {
                    m_GameObjects[screenObject.m_ObjectNumber - 1].Source = screenObject.m_ImageSources[i];
                    m_GameObjects[screenObject.m_ObjectNumber - 1].TranslateTo(screenObject.m_NewPositions[i].m_Column,
                    screenObject.m_NewPositions[i].m_Row);
                }
            }
        });
    }

    private void addImage(ScreenObjectAdd i_ScreenObject)
    {
        Image image = new Image();
                image.Source = i_ScreenObject.m_ImageSource;
   
        if(i_ScreenObject.m_Size.m_Width != 0)
        {
            image.WidthRequest= i_ScreenObject.m_Size.m_Width;
        }
   
        if(i_ScreenObject.m_Size.m_Height != 0)
        {
            image.HeightRequest= i_ScreenObject.m_Size.m_Height;
        }

        image.Aspect = Aspect.AspectFill;
        gridLayout.Add(image);
        image.TranslateTo(i_ScreenObject.m_Point.m_Column, i_ScreenObject.m_Point.m_Row);

        if(i_ScreenObject.m_ScreenObjectType != eScreenObjectType.Image)
        {
            addGameObjectToList(image, i_ScreenObject);
        }
    }

    private void addGameObjectToList(Image i_image, ScreenObjectAdd i_ScreenObject)
    {
        if(i_ScreenObject.m_ScreenObjectType == eScreenObjectType.PlayerObject)
        {
            m_PlayerObjects[i_ScreenObject.m_ObjectNumber - 1].Add(i_image);
        }
        else
        {
            m_GameObjects.Add(i_image);
        }
    }

    private void addButton(ScreenObjectAdd i_ScreenObject)
    {
            Button button = new Button();
            button.ClassId = i_ScreenObject.m_KindOfButton.ToString();
            button.HeightRequest = i_ScreenObject.m_Size.m_Height;
            button.WidthRequest = i_ScreenObject.m_Size.m_Width;
            button.CornerRadius = 70;
            gridLayout.Add(button);
            button.TranslateTo(i_ScreenObject.m_Point.m_Column, i_ScreenObject.m_Point.m_Row);
            button.Clicked += m_Game.OnButtonClicked;
    }

    async Task initializeEvents()
    {
        m_Game.AddScreenObject += addScreenObject;
        m_Game.GameObjectUpdate += gameObjectUpdate;
    }
}