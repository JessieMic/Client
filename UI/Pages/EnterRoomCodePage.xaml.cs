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

    public EnterRoomCodePage()
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
        string code = Entry.Text;
        eLoginErrors logicResponse = await r_LogicManager.CheckIfValidCode(code);

        if (logicResponse == eLoginErrors.Ok)
        {
            r_LogicManager.m_Player.RoomCode = code;

            await Shell.Current.GoToAsync(nameof(EnterNamePage));
            //await Shell.Current.GoToAsync(nameof(EnterNamePage) +
            //                              $"?{QueryIDs.k_PlayerType}={PlayerType}&" +
            //                              $"{QueryIDs.k_Code}={code}");
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
        if (r_LogicManager.m_Player.PlayerType == PlayerType.Guest)
        {
            Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync(".."));
        }
        else
        {
            Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync("../.."));
        }
    }
}