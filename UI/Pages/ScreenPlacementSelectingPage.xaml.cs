//using Objects;
//namespace UI.Pages;
//using LogicUnit;
//using Microsoft.AspNetCore.SignalR.Client;
//using Microsoft.Maui.Controls;
//using LogicUnit;
using CommunityToolkit.Mvvm.ComponentModel;
using LogicUnit;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
namespace UI.Pages;

using System.Xml.Linq;
public partial class ScreenPlacementSelectingPage : ContentPage
{
    private ScreenPlacementSelectingLogic m_pageLogic = new ScreenPlacementSelectingLogic();
    private static List<Button> m_PlacementButton = new List<Button>();
    private List<Image> m_Images = new List<Image>();
    private GameInformation m_GameInformation = GameInformation.Instance;

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
        m_pageLogic.UpdateSelectButton += visualButtonUpdate;
        m_pageLogic.ReceivedPlayerAmount += initializeButtons;
        m_pageLogic.GameIsStarting += startGame;
        initializeButtons();
    }

    async Task getScreenUpdate()
    {
        m_pageLogic.GetScreenUpdate();
    }

    private void startGame()
    {
        Shell.Current.GoToAsync("GamePage");
    }

    //protected override void OnSizeAllocated(double i_Width, double i_Height)
    //{
    
    //            base.OnSizeAllocated(i_Width,i_Height);
    //        m_ScreenSizeHeight = (int)i_Height;
    //        m_ScreenSizeWidth = (int)i_Width;
    //        m_pageLogic.SetPlayerScreenSize((int)i_Width, (int)i_Height);
    //}

    private void initializeButtons()
    {
        if (!m_Player.isInitialized)
        {
            Application.Current.Dispatcher.Dispatch(async () =>
                {
                    for (int i = 0; i < m_pageLogic.AmountOfPlayers; i++)
                    {
                        Image image = new Image();
                        Button button = new Button();
                        Position position = new Position(m_pageLogic.AmountOfPlayers, i + 1);
                        button.BorderColor = Colors.Transparent;
                        button.Text = (i + 1).ToString();
                        int a = ((m_GameInformation.m_ClientScreenDimension.SizeDTO.m_Height) / 12)/3;
                        image.HeightRequest =button.HeightRequest =  12*a;
                        image.WidthRequest=button.WidthRequest = 19*a;
                        image.Source = "placementbutton.png";
                        m_PlacementButton.Add(button);
                        m_Images.Add(image);
                        button.FontSize = a * 3;
                        button.FontAutoScalingEnabled = true;
                        button.Background = Brush.Transparent;
                        gridLayout.Add(image, (int)position.Column, (int)position.Row);
                        gridLayout.Add(m_PlacementButton[i], (int)position.Column, (int)position.Row);
                        m_PlacementButton[i].Clicked += m_pageLogic.OnButtonClicked;
                    }
                    m_Player.isInitialized = true;
                    getScreenUpdate();
                });
        }
    }
}