using LogicUnit;
using System.Xml.Linq;
namespace UI.Pages;

[QueryProperty(nameof(PlayerType), "playerType")]
[QueryProperty(nameof(RoomCode), "code")]
public partial class EnterNamePage : ContentPage
{
    private readonly LogicManager r_LogicManager;
    private ePlayerType PlayerType { get; set; }
    private string RoomCode { get; set; }

    public EnterNamePage()
    {
        InitializeComponent();
        r_LogicManager = new LogicManager();
    }

    private void OnContinueClicked(object sender, EventArgs e)
    {
        string username = Entry.Text;

        if (PlayerType == ePlayerType.Host)
        {
            r_LogicManager.CreateNewRoom(username);
        }
        else
        {
            r_LogicManager.AddPlayerToRoom(username, RoomCode);
        }
    }

    //void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    //{
    //    string oldText = e.OldTextValue;
    //    string newText = e.NewTextValue;
    //    string myText = Entry.Text;
    //}

    //void OnEntryCompleted(object sender, EventArgs e)
    //{
    //    string text = ((Entry)sender).Text;
    //}
}