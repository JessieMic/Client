using CommunityToolkit.Mvvm.ComponentModel;
using LogicUnit;

namespace UI.Pages;

[QueryProperty(nameof(PlayerType), QueryIDs.k_PlayerType)]
public partial class EnterRoomCodePage : ContentPage
{
    public string PlayerType { get; set; }

    private readonly LogicManager r_LogicManager;

    public EnterRoomCodePage()
    {
        InitializeComponent();
        r_LogicManager = new LogicManager();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string code = Entry.Text;
        eLoginErrors logicResponse = await r_LogicManager.CheckIfValidCode(code);

        if (logicResponse == eLoginErrors.Ok)
        {
            await Shell.Current.GoToAsync(nameof(EnterNamePage) +
                                          $"?{QueryIDs.k_PlayerType}={PlayerType}&" +
                                          $"{QueryIDs.k_Code}={code}");
        }
        else
        {
            ErrorLabel.Text = EnumHelper.GetDescription(logicResponse);
        }
    }
}