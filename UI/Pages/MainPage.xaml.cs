using LogicUnit;
using UI.Pages;

namespace UI
{
    public partial class MainPage : ContentPage
    {
        private LogicManager m_LogicManager;

        public MainPage()
        {
            InitializeComponent();
            m_LogicManager = new LogicManager();
        }

        private async void OnCreateRoomClicked(object sender, EventArgs e)
        {
            //Player player = new Player();
            //player.SetPlayerType(ePlayerType.Host);
            ePlayerType playerType = ePlayerType.Host;

            await Shell.Current.GoToAsync(nameof(EnterNamePage) + $"?playerType={playerType}");
        }

        private async void OnJoinRoomClicked(object sender, EventArgs e)
        {
            //Player player = new Player();
            //player.SetPlayerType(ePlayerType.Guest);
            ePlayerType playerType = ePlayerType.Guest;

            var navigationParameters =
                new Dictionary<string, object> { { "playerType", playerType } };

            //await Shell.Current.GoToAsync(nameof(EnterRoomCodePage) + $"?playerType={playerType}");
            await Shell.Current.GoToAsync(nameof(EnterRoomCodePage), navigationParameters );
        }
    }
}