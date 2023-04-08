//using Keyboard = Android.InputMethodServices.Keyboard;

namespace POC_Client;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using POC_Client.Logic;
using POC_Client.Objects;
using System.Xml.Linq;
public partial class ScreenPlacementSelectingPage : ContentPage
{
    private ScreenPlacementSelectingLogic m_pageLogic = new ScreenPlacementSelectingLogic();
    private static List<Button> m_PlacementButton = new List<Button>();
    Player m_Player = Player.Instance;

    public ScreenPlacementSelectingPage()
	{
        InitializeComponent();
        initializePage();
    }

    public static void visualButtonUpdate(object sender, VisualUpdateSelectButtons i_VisualUpdate)
    {
        m_PlacementButton[i_VisualUpdate.spot].Text = i_VisualUpdate.textOnButton;

        if (i_VisualUpdate.didPlayerSelect)
        {
            m_PlacementButton[i_VisualUpdate.spot].Background = Colors.IndianRed;
        }
        else
        {
            m_PlacementButton[i_VisualUpdate.spot].Background = default;
        }
    }

    async Task initializePage()
    {
        //m_pageLogic.UpdateSelectButton += visualButtonUpdate;
        //m_pageLogic.ReceivedPlayerAmount += initializeButtons;
        //m_pageLogic.GameIsStarting += startGame;

        initializePageLayout();
        Application.Current.Dispatcher.Dispatch(async () =>
            {
                //for (int i = 0; i < m_pageLogic.AmountOfPlayers; i++)
                //{

                //    Button button = new Button();
                //    Position position = new Position(m_pageLogic.AmountOfPlayers,i+1);

                //    button.Text = (i + 1).ToString();
                //    button.HeightRequest = 70;
                //    button.WidthRequest = 150;
                //    m_PlacementButton.Add(button);
                //    gridLayout.Add(m_PlacementButton[i], (int)position.Column ,(int)position.Row);
                //    m_PlacementButton[i].Clicked += OnButtonClicked;
                //}
                //m_Player.isInitialized = true;
                a();
            });
        m_pageLogic.UpdateSelectButton += visualButtonUpdate;
        m_pageLogic.ReceivedPlayerAmount += initializeButtons;
        m_pageLogic.GameIsStarting += startGame;

    }

    async Task getScreenUpdate()
    {
        m_pageLogic.GetScreenUpdate();
    }

    private async void startGame()
    {
        await Shell.Current.GoToAsync("GamePage");
    }

    private void initializeButtons()
    {
        if(!m_Player.isInitialized)
        {
            Application.Current.Dispatcher.Dispatch(async () =>
                {
                    //for (int i = 0; i < m_pageLogic.AmountOfPlayers; i++)
                    //{

                    //    Button button = new Button();
                    //    Position position = new Position(m_pageLogic.AmountOfPlayers,i+1);

                    //    button.Text = (i + 1).ToString();
                    //    button.HeightRequest = 70;
                    //    button.WidthRequest = 150;
                    //    m_PlacementButton.Add(button);
                    //    gridLayout.Add(m_PlacementButton[i], (int)position.Column ,(int)position.Row);
                    //    m_PlacementButton[i].Clicked += OnButtonClicked;
                    //}
                    //m_Player.isInitialized = true;
                    a();
                });
            getScreenUpdate();
        }
    }

    void a()
    {
        Button button = new Button();
        
        button.HeightRequest = 35;
        button.WidthRequest = 35;
        gridLayout.Add(button, 1, 2);

        Button button1 = new Button();

        button1.HeightRequest = 35;
        button1.WidthRequest = 35;
        gridLayout.Add(button1, 2, 1);

        Button button2 = new Button();

        button2.HeightRequest = 35;
        button2.WidthRequest = 35;
        gridLayout.Add(button2, 3, 2);

        Button button3 = new Button();

        button3.HeightRequest = 35;
        button3.WidthRequest = 35;
        gridLayout.Add(button3, 2, 3);
        
        Button button13 = new Button();
        //Image image = new Image();
        //image.Source = "q.png";
        button13.HeightRequest = 2200;
        button13.WidthRequest = 2000;
        gridLayout.Add(button13, 4, 4);
        //gridLayout.Add(new BoxView { Color = Colors.Blue }, 4,4);
    }
    private async void OnButtonClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        if (m_Player.DidPlayerPickAPlacement)
        {
            m_pageLogic.TryToDeselectScreenSpot(button.Text);
        }
        else
        {
            m_pageLogic.TryPickAScreenSpot(button.Text);
        }
    }

    private void initializePageLayout()
    {
        //gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        //gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        //gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        //gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        //gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        //gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        //gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        //gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        
        gridLayout.RowDefinitions.Add(new RowDefinition(10));
        gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(10));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));


    }
}