//using Android.Widget;
using POC_Client.Logic;
using POC_Client.Logic.Games;
using POC_Client.Objects;
using POC_Client.Objects.Enums;


namespace POC_Client.Pages;

public partial class GamePage : ContentPage
{

    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private Game m_Game;

    public GamePage()
	{
		InitializeComponent();

        m_Game = m_GameLibrary.CreateAGame(eGames.Snake);//m_GameInformation.m_NameOfGame);
        initializeEvents();
        m_Game.SetGameScreen();
        m_Game.RunGame();
    }

    public void addScreenObject(object sender,ScreenObject i_ScreenObject)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            if (i_ScreenObject.m_ScreenObjectType == eScreenObjectType.Button)
            {
                addButton(i_ScreenObject);
            }
            else if(i_ScreenObject.m_ScreenObjectType == eScreenObjectType.Image)
            {
                addImage(i_ScreenObject);
            }
            else if(i_ScreenObject.m_ScreenObjectType == eScreenObjectType.Space)
            {
                addSpace(i_ScreenObject);
            }
        });
    }

    private void addSpace(ScreenObject i_ScreenObject)
    {
        //gridLayout.ColumnDefinitions[i_ScreenObject.m_Point.m_Column].Width = i_ScreenObject.m_Size.m_Width;
        //gridLayout.RowDefinitions[i_ScreenObject.m_Point.m_Row].Height = i_ScreenObject.m_Size.m_Height;
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
        image.Opacity = 0.50;
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

    async Task initializeEvents()
    {
        m_Game.AddScreenObject += addScreenObject;
    }
}