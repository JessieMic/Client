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
        if (!m_Player.isInitialized)
        {
            Application.Current.Dispatcher.Dispatch(async () =>
                {
                    startGame();
                    for (int i = 0; i < m_pageLogic.AmountOfPlayers; i++)
                    {

                        Button button = new Button();
                        Position position = new Position(m_pageLogic.AmountOfPlayers, i + 1);

                        button.Text = (i + 1).ToString();
                        button.HeightRequest = 70;
                        button.WidthRequest = 150;
                        m_PlacementButton.Add(button);
                        gridLayout.Add(m_PlacementButton[i], (int)position.Column, (int)position.Row);
                        m_PlacementButton[i].Clicked += m_pageLogic.OnButtonClicked;
                    }
                    m_Player.isInitialized = true;
                });
            getScreenUpdate();
        }
    }
}