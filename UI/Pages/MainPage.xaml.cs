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
            m_GameInformation.init();
            m_LogicManager = new LogicManager();
        }
        private async void OnSkipClicked(object sender, EventArgs e)
        {
            m_GameInformation.m_NameOfGame = Objects.Enums.eGames.Pacman;
            m_GameInformation.AmountOfPlayers = 2;
            m_GameInformation.Player.Name = DateTime.Now.ToString();
            await Shell.Current.GoToAsync(nameof(ScreenPlacementSelectingPage));
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            addComponents();
        }

        protected override void OnSizeAllocated(double i_Width, double i_Height)
        {
            base.OnSizeAllocated(i_Width, i_Height);
            m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height = (int)i_Height;
            m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width = (int)i_Width;
        }

        private async void OnCreateRoomClicked(object sender, EventArgs e)
        {
            m_GameInformation.Player.Name = string.Empty;
            m_GameInformation.Player.PlayerType = PlayerType.Host;
            await Shell.Current.GoToAsync(nameof(EnterNamePage));
            //await Shell.Current.GoToAsync(nameof(EnterNamePage) +
            //                              $"?{QueryIDs.k_PlayerType}={PlayerType.k_Host}");
        }

        private async void OnJoinRoomClicked(object sender, EventArgs e)
        {
            m_GameInformation.Player.Name = string.Empty;
            m_GameInformation.Player.PlayerType = PlayerType.Guest;
            await Shell.Current.GoToAsync(nameof(EnterRoomCodePage));
            //await Shell.Current.GoToAsync(nameof(EnterRoomCodePage) +
            //                              $"?{QueryIDs.k_PlayerType}={PlayerType.k_Guest}");
        }

        private void addComponents()
        {
            //ButtonImage skipBtn, createRoomBtn, joinRoomBtn;
            createButtonImage("SKIP:)", OnSkipClicked, 1, 1);
            createButtonImage("Create a Room", OnCreateRoomClicked, 1, 2);
            createButtonImage("Join a Room", OnJoinRoomClicked, 1, 3);
        }

        private void createButtonImage(string i_Text, EventHandler m_ClickEvent, int i_Row, int i_Col)
        {
            ButtonImage btn = new ButtonImage();
            //btn.GetButton().WidthRequest = 1000;
            //btn.GetImage().WidthRequest = 1000;
            btn.HorizontalOptions = LayoutOptions.CenterAndExpand;
            btn.VerticalOptions = LayoutOptions.CenterAndExpand;
            btn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
            btn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;

            btn.Text = i_Text;
            btn.Source = "entrance_btn.PNG";
            btn.GetButton().Clicked += m_ClickEvent;
            buttonComponent.Add(btn.GetImage(), i_Row, i_Col);
            buttonComponent.Add(btn.GetButton(), i_Row, i_Col);
        }
    }
}