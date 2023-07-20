using LogicUnit;
using Objects;
using Objects.Enums;
using UI.Pages;

namespace UI
{
    public partial class MainPage : ContentPage
    {
        private LogicManager m_LogicManager;
        private GameInformation m_GameInformation = GameInformation.Instance;

        public MainPage()
        {
            InitializeComponent();
            m_LogicManager = new LogicManager();
        }

        protected override void OnSizeAllocated(double i_Width, double i_Height)
        {

            base.OnSizeAllocated(i_Width, i_Height);
            m_GameInformation.m_ClientScreenDimension.m_OurSize.m_Height = (int)i_Height;
            m_GameInformation.m_ClientScreenDimension.m_OurSize.m_Width = (int)i_Width;
           // m_pageLogic.SetPlayerScreenSize((int)i_Width, (int)i_Height);
        }

        private async void OnCreateRoomClicked(object sender, EventArgs e)
        {
            m_LogicManager.m_Player.PlayerType = PlayerType.Host;
            await Shell.Current.GoToAsync(nameof(EnterNamePage));
            //await Shell.Current.GoToAsync(nameof(EnterNamePage) +
            //                              $"?{QueryIDs.k_PlayerType}={PlayerType.k_Host}");
        }

        private async void OnJoinRoomClicked(object sender, EventArgs e)
        {
            m_LogicManager.m_Player.PlayerType = PlayerType.Guest;
            await Shell.Current.GoToAsync(nameof(EnterRoomCodePage));
            //await Shell.Current.GoToAsync(nameof(EnterRoomCodePage) +
            //                              $"?{QueryIDs.k_PlayerType}={PlayerType.k_Guest}");
        }

        private async void OnSkipClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ScreenPlacementSelectingPage));
        }
    }
}