using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using LogicUnit;
using Objects;
using Objects.Enums;
using UI.Pages.LobbyPages;

namespace UI.Pages;

//[QueryProperty(nameof(PlayerType), QueryIDs.k_PlayerType)]
public partial class EnterRoomCodePage : ContentPage
{
    //public string PlayerType { get; set; }

    private readonly LogicManager r_LogicManager;
    private GameInformation m_GameInformation = GameInformation.Instance;

    public EnterRoomCodePage()
    {
        InitializeComponent();
        r_LogicManager = new LogicManager();
        placeContinueButton();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        //placeContinueButton();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string code = Entry.Text;
        eLoginErrors logicResponse = await r_LogicManager.CheckIfValidCode(code);

        if (logicResponse == eLoginErrors.Ok)
        {
            logicResponse = await r_LogicManager.CheckIfHostLeftForLogin(code);
        }

        if (logicResponse == eLoginErrors.Ok)
        {
            m_GameInformation.Player.RoomCode = code;

            await Shell.Current.GoToAsync(nameof(EnterNamePage));
            //await Shell.Current.GoToAsync(nameof(EnterNamePage) +
            //                              $"?{QueryIDs.k_PlayerType}={PlayerType}&" +
            //                              $"{QueryIDs.k_Code}={code}");
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
        if (m_GameInformation.Player.PlayerType == PlayerType.Guest)
        {
            Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync(".."));
        }
        else
        {
            Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync("../.."));
        }
    }
}