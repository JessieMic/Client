//using Android.Widget;
using POC_Client.Logic;
using POC_Client.Objects;

namespace POC_Client.Pages;

public partial class GamePage : ContentPage
{
    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private Game m_Game = null;

    public GamePage()
	{
		InitializeComponent();
        initializeEvents();
        m_Game = m_GameLibrary.CreateAGame(m_GameInformation.m_NameOfGame);
        m_Game.RunGame();
    }

    private void initializeEvents()
    {
        Application.Current.Dispatcher.Dispatch(async () =>
            {

                Button button = new Button();

                button.HeightRequest = 35;
                button.WidthRequest = 35;
                button.BackgroundColor = Colors.Red;
                gridLayout.Add(button, 1, 2);

                Button button1 = new Button();

                button1.HeightRequest = 35;
                button1.WidthRequest = 35;
                button1.BackgroundColor = Colors.Green;
                gridLayout.Add(button1, 2, 1);

                Button button2 = new Button();

                button2.HeightRequest = 35;
                button2.WidthRequest = 35;
                button2.BackgroundColor = Colors.Blue;
                gridLayout.Add(button2, 3, 2);

                Button button3 = new Button();

                button3.HeightRequest = 35;
                button3.WidthRequest = 35;
                gridLayout.Add(button3, 2, 3);

                Image image = new Image();
                image.Source = "aa.png";
                
                image.WidthRequest = 585;
                gridLayout.Add(image,4,0);
      
            });
    }
}


//Application.Current.Dispatcher.Dispatch(async () =>
//        {
////    Button button = new Button();

////    button.HeightRequest = 35;
////    button.WidthRequest = 35;
////    gridLayout.Add(button, 1, 2);

////    Button button1 = new Button();

////    button1.HeightRequest = 35;
////    button1.WidthRequest = 35;
////    gridLayout.Add(button1, 2, 1);

////    Button button2 = new Button();

////    button2.HeightRequest = 35;
////    button2.WidthRequest = 35;
////    gridLayout.Add(button2, 3, 2);

////    Button button3 = new Button();

////    button3.HeightRequest = 35;
////    button3.WidthRequest = 35;
////    gridLayout.Add(button3, 2, 3);

////    Button button13 = new Button();
////    Image image = new Image();
////    image.Source = "aa.png";
////    //image.BackgroundColor = Colors.Blue;
////    //image.Scale=1000;
////    button13.HeightRequest = 2200;
////    button13.WidthRequest = 2000;
////    button13.ImageSource = "dotnet_bot.png";
////    //gridLayout.Add(button13, 4, 4);
////    //gridLayout.Add(image,4,3);
////    //gridLayout.Add(new BoxView { Color = Colors.Blue }, 4,4);
//        });