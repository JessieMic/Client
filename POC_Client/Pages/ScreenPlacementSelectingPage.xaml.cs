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


        amountOfButtons = 5;//m_pageLogic.AmountOfPlayers;

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