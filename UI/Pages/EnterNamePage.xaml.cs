using CommunityToolkit.Maui.Views;
using LogicUnit;
using Objects;
using Objects.Enums;
using CommunityToolkit.Maui.Core.Extensions;
//using System.Xml.Linq;
//using static ObjCRuntime.Dlfcn;
using UI.Pages.LobbyPages;

namespace UI.Pages;

//[QueryProperty(nameof(PlayerType), QueryIDs.k_PlayerType)]
//[QueryProperty(nameof(RoomCode), QueryIDs.k_Code)]
public partial class EnterNamePage : ContentPage
{
    private readonly LogicManager r_LogicManager;
    private GameInformation m_GameInformation = GameInformation.Instance;
    //public string PlayerType { get; set; }
    //public string RoomCode { get; set; }

    public EnterNamePage()
    {
        InitializeComponent();
        r_LogicManager = new LogicManager();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        placeContinueButton();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string username = Entry.Text;
        eLoginErrors logicResponse;// = eLoginErrors.Ok;
        //eLoginErrors logicResponse =0;

        if (m_GameInformation.Player.PlayerType == PlayerType.Host)
        {
            logicResponse = await r_LogicManager.CreateNewRoom(username);

            if (logicResponse == eLoginErrors.Ok)
            {
                m_GameInformation.Player.RoomCode = r_LogicManager.GetRoomCode();
            }
        }
        else
        {
            logicResponse = await r_LogicManager.AddPlayerToRoom(username, m_GameInformation.Player.RoomCode);
        }

        if (logicResponse == eLoginErrors.Ok)
        {
            m_GameInformation.Player.Name = username;

            await Shell.Current.GoToAsync(nameof(Lobby));
            //await Shell.Current.GoToAsync(nameof(Lobby) +
            //    $"?{QueryIDs.k_PlayerType}={PlayerType}&" +
            //    $"{QueryIDs.k_Code}={RoomCode}&" +
            //    $"{QueryIDs.k_Name}={username}");
        }
        else if (logicResponse == eLoginErrors.ServerError)
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
        continueBtn.Text = "Continue";
        continueBtn.GetButton().Clicked += OnContinueClicked;
        continueBtn.VerticalOptions = LayoutOptions.CenterAndExpand;
        continueBtn.HorizontalOptions = LayoutOptions.CenterAndExpand;
        continueBtn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
        continueBtn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;
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