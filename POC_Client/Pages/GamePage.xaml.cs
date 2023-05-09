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
    //private List<List<Image>> m_PlayerObjects = new List<List<Image>>();
    //private List<Image> m_GameObjects = new List<Image>();

    public GamePage()
	{
		InitializeComponent();
        initializePage();
        m_Game.RunGame();
    }

    private void initializePage()
    {
        m_Game = m_GameLibrary.CreateAGame(eGames.Pacman);
       // m_Game = m_GameLibrary.CreateAGame(eGames.Snake);//m_GameInformation.m_NameOfGame);
        initializeEvents();
        initializeGame();
        
    }

    private void initializeGame()
    {
        m_Game.InitializeGame();
    }

    public void addGameObjects(object sender, List<Image> i_GameObjectsToAdd)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
            {
                //gridLayout.Add(m_Game.a);
                foreach (var image in i_GameObjectsToAdd)
                {
                    if(image.ClassId != String.Empty)
                    {
                        addButton(image);
                    }
                    else
                    {
                        gridLayout.Add(image);
                    }
                }
            });
    }

    private void addButton(Image i_button)
    {
        Button button = new Button();
        button.ClassId = i_button.ClassId;//.m_ButtonType.ToString();//.m_KindOfButton.ToString();
        button.HeightRequest = i_button.HeightRequest;//i_ButtonToAdd.m_Size.m_Height;
        button.WidthRequest = i_button.WidthRequest;//i_ButtonToAdd.m_Size.m_Width;
        button.TranslationX = i_button.TranslationX;
        button.TranslationY = i_button.TranslationY;
        button.CornerRadius = 70;
        gridLayout.Add(button);
        //button.TranslateTo(i_ButtonToAdd.m_PointsOnScreen[0].m_Column, i_ButtonToAdd.m_PointsOnScreen[0].m_Row);
        button.Clicked += m_Game.OnButtonClicked;
    }


    public void deleteObject(object sender, GameObject i_ObjectToDelete)
    {
        //if(i_ObjectToDelete.m_ScreenObjectType == eScreenObjectType.Player)
        //{
        //    foreach(var image in m_PlayerObjects[i_ObjectToDelete.m_ObjectNumber - 1])
        //    {
        //        image.FadeTo(0, 700, null);
        //        //gridLayout.Remove(image);
        //    }

        //    m_PlayerObjects[i_ObjectToDelete.m_ObjectNumber - 1].Clear();
        //}
        //else
        //{
        //    gridLayout.Remove(m_GameObjects[i_ObjectToDelete.m_ObjectNumber - 1]);
        //}
    }

    async Task initializeEvents()
    {
        m_Game.AddGameObjectList += addGameObjects;
        //m_Game.GameObjectsUpdate += gameObjectsUpdate;
        //m_Game.GameObjectToDelete += deleteObject;
    }
}