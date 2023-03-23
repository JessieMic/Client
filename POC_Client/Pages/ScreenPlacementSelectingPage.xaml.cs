namespace POC_Client;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using POC_Client.Logic;
using POC_Client.Objects;
using System.Xml.Linq;
public partial class ScreenPlacementSelectingPage : ContentPage
{
    private ScreenPlacementSelectingLogic m_pageLogic = new ScreenPlacementSelectingLogic();
    //static Button[] m_PlacementButton;
    static List<Button> m_PlacementButton = new List<Button>();
    //public readonly HubConnection r_ConnectionToServer;
    ClientInfo m_ClientInfo = ClientInfo.Instance;

    public ScreenPlacementSelectingPage()
	{

        InitializeComponent();
        initializePage();

        //try
        //{
        //    r_ConnectionToServer = new HubConnectionBuilder()
        //        .WithUrl(Utils.m_GameHubAddress).Build();
        //    //m_ClientInfo.DidClientPickAPlacement = false;

        //    //****add to server
        //    r_ConnectionToServer.On<int>("ReceiveAmountOfPlayers", (i_AmountOfPlayers) =>
        //        {
        //            MainThread.BeginInvokeOnMainThread(() =>
        //            {
        //                m_pageLogic.AmountOfPlayers = i_AmountOfPlayers;
        //            });
        //        });

        //    r_ConnectionToServer.On<string[]>("RecieveScreenUpdate", (buttonsThatAreOccupied) =>
        //        {
        //            MainThread.BeginInvokeOnMainThread(() =>
        //            {
        //                int buttonNumber = 0;
        //                foreach (string element in buttonsThatAreOccupied)
        //                {
        //                    if (buttonsThatAreOccupied[buttonNumber] != String.Empty
        //                        && buttonsThatAreOccupied[buttonNumber] != null)
        //                    {
        //                        m_PlacementButton[buttonNumber].Text = buttonsThatAreOccupied[buttonNumber];
        //                        m_PlacementButton[buttonNumber].Background = Colors.IndianRed;
        //                    }
        //                    buttonNumber++;
        //                }
        //            });
        //        });

        //    r_ConnectionToServer.On<string, int, double, double>
        //    ("PlacementUpdateRecevied", (i_NameOfClientThatGotASpot, i_Spot, width, height) =>
        //        {
        //            MainThread.BeginInvokeOnMainThread(() =>
        //                {
        //                    visualButtonUpdate(i_Spot, i_NameOfClientThatGotASpot, true);
        //                    m_pageLogic.m_AmountOfPlayerThatAreConnected++;

        //                    if (m_ClientInfo.Name == i_NameOfClientThatGotASpot)
        //                    {
        //                        m_ClientInfo.ButtonThatClientPicked = 1 + i_Spot;
        //                        m_ClientInfo.DidClientPickAPlacement = true;
        //                    }

        //                    if (m_pageLogic.AreAllTheUsersReady())
        //                    {
        //                        r_ConnectionToServer.InvokeAsync("GameIsAboutToStart");
        //                    }
        //                });
        //        });

        //    r_ConnectionToServer.On("StartGame", async () =>
        //    {
        //        await Shell.Current.GoToAsync($"GameRoomPage?");
        //    });

        //    r_ConnectionToServer.On<string, int>("DeSelectPlacementUpdatReceived", (i_NameOfClientThatDeselected, i_Spot) =>
        //        {
        //            MainThread.BeginInvokeOnMainThread(() =>
        //                {
        //                    m_pageLogic.m_AmountOfPlayerThatAreConnected--;

        //                    if (m_ClientInfo.Name == i_NameOfClientThatDeselected)
        //                    {
        //                        m_ClientInfo.ButtonThatClientPicked = 0;
        //                        m_ClientInfo.DidClientPickAPlacement = false;
        //                    }
        //                });
        //        });

        //    Task.Run(() =>
        //        {
        //            Application.Current.Dispatcher.Dispatch(async () =>
        //                {
        //                    await r_ConnectionToServer.StartAsync();
        //                    await getAmountOfPlayersFromServer();
        //                    initializePage();
        //                });
        //        });
        //}
        //catch
        //{ }
        m_pageLogic.UpdateSelectButton += visualButtonUpdate;
    }

    void main()
    {
        initializeButtons();
        getScreenUpdate();
    }

    public static void visualButtonUpdate(object sender, VisualUpdateSelectButtons i_VisualUpdate)
    {
        m_PlacementButton[i_VisualUpdate.spot].Text = i_VisualUpdate.textOnButton;

        if (i_VisualUpdate.didClientSelect)
        {
            m_PlacementButton[i_VisualUpdate.spot].Background = Colors.IndianRed;
        }
        else
        {
            m_PlacementButton[i_VisualUpdate.spot].Background = default;
        }
    }

    //private void visualButtonUpdate(VisualUpdateSelectButtons(int i_ButtonsSpot, string i_NewTextOnButton,bool i_didClientSelect)
    //{
    //    m_PlacementButton[i_ButtonsSpot].Text = i_NewTextOnButton;

    //    if(i_didClientSelect)
    //    {
    //        m_PlacementButton[i_ButtonsSpot].Background = Colors.IndianRed;
    //    }
    //    else
    //    {
    //        m_PlacementButton[i_ButtonsSpot].Background = default;
    //    }
    //}

    async Task initializePage()
    {
        initializePageLayout();
        initializeButtons();
        getScreenUpdate();
    }

    async Task getScreenUpdate()
    {
        m_pageLogic.GetScreenUpdate();
    }

    private void initializePageLayout()
    {
        gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        gridLayout.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        gridLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
    }

    private void initializeButtons()
    {
        int amountOfButtons;

        //while(m_pageLogic.AmountOfPlayers == 0){}

        amountOfButtons = 5;//m_pageLogic.AmountOfPlayers;

        //m_PlacementButton = new Button[amountOfButtons];

        for(int i = 0; i < amountOfButtons; i++)
        {

            Button button = new Button();

            button.Text = (i + 1).ToString();
            button.HeightRequest = 70;
            button.WidthRequest = 150; 
            m_PlacementButton.Add(button);
            gridLayout.Add(m_PlacementButton[i], m_pageLogic.GetButtonColumnValue(i+1),m_pageLogic.GetButtonRowValue(i+1));
            m_PlacementButton[i].Clicked += OnButtonClicked;
        }
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        if (m_ClientInfo.DidClientPickAPlacement)
        {
            //await r_ConnectionToServer.InvokeCoreAsync("TryToDeselectScreenSpot", args: new[]
            //    {m_ClientInfo.Name,m_ClientInfo.ButtonThatClientPicked.ToString(),button.Text});
        }
        else
        {
            m_pageLogic.TryPickAScreenSpot(button.Text);
        }
    }

    private async Task getAmountOfPlayersFromServer()
    {
        //await r_ConnectionToServer.InvokeCoreAsync(
        //    "SendTheAmountOfPlayers",
        //    args: new[] { r_ConnectionToServer.ConnectionId });
    }
}