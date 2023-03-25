using LogicUnit;

namespace UI.Pages;

[QueryProperty(nameof(PlayerType), "playerType")]
public partial class EnterRoomCodePage : ContentPage
{
    private ePlayerType PlayerType { get; set; }
    private readonly LogicManager r_LogicManager;

    public EnterRoomCodePage()
    {
        InitializeComponent();
        r_LogicManager = new LogicManager();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string code = Entry.Text;
        eLoginErrors logicResponse = r_LogicManager.CheckIfValidCode(code);

        if (logicResponse == eLoginErrors.Ok)
        {
            await Shell.Current.GoToAsync(nameof(EnterNamePage) +
                                          $"?playerType={PlayerType}&" +
                                          $"code={code}");
        }
        else
        {
            ErrorLabel.Text = EnumHelper.GetDescription(logicResponse);
        }
    }
}