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
    Image pLabel = new Image();

    public GamePage()
	{
		InitializeComponent();

        m_Game = m_GameLibrary.CreateAGame(eGames.Snake);//m_GameInformation.m_NameOfGame);
        initializeEvents();
        m_Game.SetGameScreen();
        m_Game.RunGame();
    }

    public void addScreenObject(object sender, List<ScreenObject> i_ScreenObject)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            foreach(var screenObject in i_ScreenObject)
            {
                if (screenObject.m_ScreenObjectType == eScreenObjectType.Button)
                {
                    addButton(screenObject);
                }
                else if (screenObject.m_ScreenObjectType == eScreenObjectType.Image)
                {
                    addImage(screenObject);
                }
            }
            pLabel.HeightRequest = 35;
            pLabel.WidthRequest = 35;
            pLabel.Source = "player.png";
            gridLayout.Add(pLabel);
        });
    }

    private void addImage(ScreenObject i_ScreenObject)
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
        //image.Opacity = 0.50;
        image.BackgroundColor = Colors.Blue;
        gridLayout.Add(image);//,i_ScreenObject(.m_Point.m_Column,i_ScreenObject.m_Point.m_Row);
        image.TranslateTo(i_ScreenObject.m_Point.m_Column, i_ScreenObject.m_Point.m_Row);
    }

    private void addButton(ScreenObject i_ScreenObject)
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

    private void gameObjectUpdate(object sender, Point p)
    {
        pLabel.TranslationX = p.m_Column;
        pLabel.TranslationY = p.m_Row;
    }

    async Task initializeEvents()
    {
        m_Game.AddScreenObject += addScreenObject;
        m_Game.GameObjectUpdate += gameObjectUpdate;
    }
}