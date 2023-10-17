using CommunityToolkit.Maui.Views;
using LogicUnit;
using Objects;
using Objects.Enums;
using CommunityToolkit.Maui.Core.Extensions;
//using System.Xml.Linq;
//using static ObjCRuntime.Dlfcn;
using UI.Pages.LobbyPages;
using CommunityToolkit.Maui.Storage;

namespace UI.Pages;

//[QueryProperty(nameof(PlayerType), QueryIDs.k_PlayerType)]
//[QueryProperty(nameof(RoomCode), QueryIDs.k_Code)]
public partial class EnterNamePage : ContentPage
{
    private readonly LogicManager r_LogicManager;
    private GameInformation m_GameInformation = GameInformation.Instance;
    private string m_Path = FileSystem.Current.AppDataDirectory;
    private string m_FullPath;
    //public string PlayerType { get; set; }
    //public string RoomCode { get; set; }

    public EnterNamePage()
    {
        m_FullPath = Path.Combine(m_Path, "PlayerName.txt");
        InitializeComponent();
        r_LogicManager = new LogicManager();
        placeContinueButton();
        if (File.Exists(m_FullPath))
        {
            Entry.Text = File.ReadAllText(m_FullPath); 
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        //placeContinueButton();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string username = Entry.Text;
        eLoginErrors logicResponse;// = eLoginErrors.Ok;
        bool showCodePopup = false;
        //eLoginErrors logicResponse =0;

        if (m_GameInformation.Player.PlayerType == PlayerType.Host)
        {
            logicResponse = await r_LogicManager.CreateNewRoom(username);

            if (logicResponse == eLoginErrors.Ok)
            {
                File.WriteAllText(m_FullPath, username);
                m_GameInformation.Player.RoomCode = r_LogicManager.GetRoomCode();
                showCodePopup = true;
            }
        }
        else
        {
            logicResponse = await r_LogicManager.CheckIfHostLeftForLogin(m_GameInformation.Player.RoomCode);
            if (logicResponse == eLoginErrors.Ok)
            {
                logicResponse = await r_LogicManager.AddPlayerToRoom(username, m_GameInformation.Player.RoomCode);
                showCodePopup = false;
            }
        }

        if (logicResponse == eLoginErrors.Ok)
        {
            m_GameInformation.Player.Name = username;

            await Shell.Current.GoToAsync($"{nameof(Lobby)}?ShowCodePopup={showCodePopup}");
            //await Shell.Current.GoToAsync(nameof(Lobby) +
            //    $"?{QueryIDs.k_PlayerType}={PlayerType}&" +
            //    $"{QueryIDs.k_Code}={RoomCode}&" +
            //    $"{QueryIDs.k_Name}={username}");
        }
        else if (logicResponse == eLoginErrors.ServerError || logicResponse == eLoginErrors.RoomClosed)
        {
            MessagePopUp messagePopUp = new MessagePopUp(goToMainPage, EnumHelper.GetDescription(logicResponse));
            Application.Current.Dispatcher.Dispatch(() =>
            {
                this.ShowPopup(messagePopUp);
            });
        }
        else
        {
            ErrorLabel.Text = EnumHelper.GetDescription(logicResponse);
        }
    }

    private void placeContinueButton()
    {
        ButtonImage continueBtn = new ButtonImage();
        double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

        continueBtn.Text = "Continue";
        continueBtn.GetButton().Clicked += OnContinueClicked;
        continueBtn.WidthRequest = 0.5 * width;
        continueBtn.HeightRequest = 0.2 * height;
        continueBtn.FontSize = 0.3 * 0.2 * height;
        continueBtn.Source = "entrance_btn.PNG";
        objectsComponent.Add(continueBtn.GetImage(), 1, 3);
        objectsComponent.Add(continueBtn.GetButton(), 1, 3);
    }

    private void goToMainPage()
    {
        if (m_GameInformation.Player.PlayerType == PlayerType.Host)
        {
            Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync(".."));
        }
        else
        {
            Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync("../.."));
        }
    }
}