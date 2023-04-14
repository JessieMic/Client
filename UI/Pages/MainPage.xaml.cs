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
            await Shell.Current.GoToAsync(nameof(EnterNamePage) +
                                          $"?{QueryIDs.k_PlayerType}={PlayerType.k_Host}");
        }

        private async void OnJoinRoomClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(EnterRoomCodePage) +
                                          $"?{QueryIDs.k_PlayerType}={PlayerType.k_Guest}");
        }
    }
}