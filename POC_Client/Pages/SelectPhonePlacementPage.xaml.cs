//using Android.Hardware.Lights;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using POC_Client.Logic;

//using static Android.Icu.Text.ListFormatter;

namespace POC_Client;

[QueryProperty(nameof(Name), "name")]

public partial class SelectPhonePlacementPage : ContentPage
{
    private readonly HubConnection r_ConnectionToServer;
   
    public string Name { get; set; }

    public SelectPhonePlacementPage()
    {
        InitializeComponent();
        initializeButtonList();
        //GetScreenSize();

        r_ConnectionToServer = new HubConnectionBuilder()
            .WithUrl(Utils.m_GameHubAddress)
            .Build();

        r_ConnectionToServer.On("StartGame", () =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                enterGameRoom();
            });
        });

        r_ConnectionToServer.On<string, int, double, double>
            ("PlacementUpdateRecevied", (nameOfClientThatGotASpot, spot, width, height) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
                {
                    int i = 0;
                    i++;
                    //m_Buttons[spot].Text = "s";
                    //m_Buttons[spot].Background = Colors.IndianRed;
                    //m_Dimentions[spot].Width = width;
                    //m_Dimentions[spot].Height = height;
                    //m_AmountOfPlayerThatAreConnected++;

                    //if (Name.Equals(nameOfClientThatGotASpot))
                    //{
                    //    m_TheButtonUserPicked = (1 + spot).ToString();
                    //    m_DidUserPick = true;
                    //}

                    //if (m_TheButtonUserPicked.Equals("1"))
                    //{
                    //    if (m_AmountOfPlayerThatAreConnected == 4)
                    //    {

                    //        r_ConnectionToServer.InvokeAsync("GameIsAboutToStart");
                    //    }
                    //}
                });

            int io = 0;
            io++;
        });

        r_ConnectionToServer.On<string, int>("DeSelectPlacementUpdatReceived", (nameOfClientThatDeselected, spot) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                int i = 0;
                i++;
                //m_Buttons[spot].Text = (1 + spot).ToString();
                //m_Buttons[spot].Background = default;
                //m_Dimentions[spot].Width = 0;
                //m_Dimentions[spot].Height = 0;
                //m_AmountOfPlayerThatAreConnected--;

                //if (Name.Equals(nameOfClientThatDeselected))
                //{
                //    m_TheButtonUserPicked = string.Empty;
                //    m_DidUserPick = false;
                //}
            });
        });

        r_ConnectionToServer.On<string[]>("RecieveScreenUpdate", (buttonsThatAreOccupied) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                int buttonNumber = 0;
                foreach (string element in buttonsThatAreOccupied)
                {
                    if (buttonsThatAreOccupied[buttonNumber] != String.Empty
                    && buttonsThatAreOccupied[buttonNumber] != null)
                    {
                        //m_Buttons[buttonNumber].Text = buttonsThatAreOccupied[buttonNumber];
                        //m_Buttons[buttonNumber].Background = Colors.IndianRed;
                    }
                    buttonNumber++;
                }
            });
        });

            Task.Run(() =>
        {
            Dispatcher.Dispatch(async () =>
            {
                await r_ConnectionToServer.StartAsync();
                ScreenPlacementSelectingLogic i = new ScreenPlacementSelectingLogic();
                await getScreenUpdate();
            });
        });
    }

    async Task getScreenUpdate()
    {
        await r_ConnectionToServer.InvokeCoreAsync("RequestScreenUpdate", args: new[]
           {r_ConnectionToServer.ConnectionId});
    }

    protected override async void OnSizeAllocated(double width, double height)
    {
        //base.OnSizeAllocated(width, height);
        //m_ScreenWidth = width.ToString();
        //m_ScreenHeight = height.ToString();
    }

    async void enterGameRoom()
    {


        //m_W1 = m_Dimentions[1].Width;
        //m_W2 = m_Dimentions[2].Width;
        //m_H1 = m_Dimentions[1].Height;
        //m_H2 = m_Dimentions[2].Height;
        //m_W0 = m_Dimentions[0].Width;
        //m_H0 = m_Dimentions[0].Height;
        //m_H3 = m_Dimentions[3].Height;
        //m_W3 = m_Dimentions[3].Width;
        //await Shell.Current.GoToAsync($"GameRoomPage?spot={theButtonUserPicked}&totalWidth={totalWidth}&totalHeight={totalHeight}&mainWidth={mainScreenWidth}&mainHeight={mainScreenHeight}&valuesToAddToConnectTheScreens={valuesToAddToConnectTheScreens}&values={valuesToAddToConnectTheScreen}&h0={h0}&h1={h1}&h2={h2}&w0={w0}&w1={w1}&w2={w2}");// navParam);//
        //await Shell.Current.GoToAsync($"GameRoomPage?spot={m_TheButtonUserPicked}&h3={m_H3}&w3={m_W3}&h0={m_H0}&h1={m_H1}&h2={m_H2}&w0={m_W0}&w1={m_W1}&w2={m_W2}");// navParam);//
    }

    private void initializeButtonList()
    {
        //m_Buttons.Add(Button1);
        //m_Buttons.Add(Button2);
        //m_Buttons.Add(Button3);
        //m_Buttons.Add(Button4);
        
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        try
        {
            Button button = sender as Button;

            //if (m_DidUserPick) //User has already picked a screen spot
            //{
            //    await r_ConnectionToServer.InvokeCoreAsync("TryToDeselectScreenSpot", args: new[]
            //{Name,"1","a"});
            //}
            //else
            {
                await r_ConnectionToServer.InvokeCoreAsync("TryPickAScreenSpot", args: new[]
            {Name,button.Text,"1","1"});
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Problem with trying to select a button");
        }
    }
}