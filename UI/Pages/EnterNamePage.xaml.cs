using LogicUnit;
//using System.Xml.Linq;
//using static ObjCRuntime.Dlfcn;

namespace UI.Pages;

[QueryProperty(nameof(PlayerType), QueryIDs.k_PlayerType)]
[QueryProperty(nameof(RoomCode), QueryIDs.k_Code)]
public partial class EnterNamePage : ContentPage
{
    private readonly LogicManager r_LogicManager;
    public string PlayerType { get; set; }
    public string RoomCode { get; set; }

    public EnterNamePage()
    {
        InitializeComponent();
        r_LogicManager = new LogicManager();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string username = Entry.Text;
        eLoginErrors logicResponse;

        if (PlayerType == LogicUnit.PlayerType.k_Host)
        {
            logicResponse = await r_LogicManager.CreateNewRoom(username);

            if (logicResponse == eLoginErrors.Ok)
            {
                RoomCode = r_LogicManager.GetRoomCode();
            }
        }
        else
        {
            logicResponse = r_LogicManager.AddPlayerToRoom(username, RoomCode);
        }

        if (logicResponse == eLoginErrors.Ok)
        {
            // go to next page with the code
        }
        else
        {
            ErrorLabel.Text = EnumHelper.GetDescription(logicResponse);
        }
    }
}