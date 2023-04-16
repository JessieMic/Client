using System.Threading;
using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Maui.Core.Platform;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic;
using POC_Client.Objects;

namespace POC_Client;

public partial class MainPage : ContentPage
{
    private readonly HubConnection r_Connection;
    Player m_PlayerInfo = Player.Instance;

    public MainPage()
	{
        InitializeComponent();
        //StatusBar.SetColor(Colors.Transparent);
        m_PlayerInfo.Name = DateAndTime.TimeString;
        Shell.Current.GoToAsync("ScreenPlacementSelectingPage");
    }

    //async void OnReadyClicked(object sender, EventArgs e)
    //{
    //    if (!entry.Text.Equals(string.Empty))
    //    {
    //        m_PlayerInfo.Name = DateAndTime.TimeString;
    //        await Shell.Current.GoToAsync("ScreenPlacementSelectingPage");
    //    }
    //}

    //void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    string oldText = e.OldTextValue;
    //    string newText = e.NewTextValue;
    //    string myText = entry.Text;
    //}

    void OnEntryCompleted(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
    }
}

