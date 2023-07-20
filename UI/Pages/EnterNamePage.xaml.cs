using LogicUnit;
using Objects.Enums;
//using System.Xml.Linq;
//using static ObjCRuntime.Dlfcn;
using UI.Pages.LobbyPages;

namespace UI.Pages;

//[QueryProperty(nameof(PlayerType), QueryIDs.k_PlayerType)]
//[QueryProperty(nameof(RoomCode), QueryIDs.k_Code)]
public partial class EnterNamePage : ContentPage
{
    private readonly LogicManager r_LogicManager;
    //public string PlayerType { get; set; }
    //public string RoomCode { get; set; }

    public EnterNamePage()
    {
        InitializeComponent();
        r_LogicManager = new LogicManager();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string username = Entry.Text;
        eLoginErrors logicResponse =0;

        if (r_LogicManager.m_Player.PlayerType == PlayerType.Host)
        {
            logicResponse = await r_LogicManager.CreateNewRoom(username);

            if (logicResponse == eLoginErrors.Ok)
            {
                r_LogicManager.m_Player.RoomCode = r_LogicManager.GetRoomCode();
            }
        }
        else
        {
            logicResponse = await r_LogicManager.AddPlayerToRoom(username, r_LogicManager.m_Player.RoomCode);
        }

        if (logicResponse == eLoginErrors.Ok)
        {
            r_LogicManager.m_Player.Name = username;

            await Shell.Current.GoToAsync(nameof(Lobby));
            //await Shell.Current.GoToAsync(nameof(Lobby) +
            //    $"?{QueryIDs.k_PlayerType}={PlayerType}&" +
            //    $"{QueryIDs.k_Code}={RoomCode}&" +
            //    $"{QueryIDs.k_Name}={username}");
        }
        else
        {
            ErrorLabel.Text = EnumHelper.GetDescription(logicResponse);
        }
    }
}