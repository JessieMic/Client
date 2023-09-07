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

        private async void OnSkipClicked(object sender, EventArgs e)
        {
           m_GameInformation.m_NameOfGame = Objects.Enums.eGames.Pong;
    //        m_GameInformation.m_NameOfGame = Objects.Enums.eGames.BombIt;
            //m_GameInformation.m_NameOfGame = Objects.Enums.eGames.Pacman;
            m_GameInformation.AmountOfPlayers =4;
            m_GameInformation.Player.Name = DateTime.Now.Millisecond.ToString();
            //var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            //m_GameInformation.Player.Name = mainDisplayInfo.Density.ToString();

            //TODO: remove once done:
            LogicUnit.ServerAddressManager.Instance!.SetAddresses("http://192.116.98.113:44305");//"http://localhost:5163" );//(
                                                                                                 // LogicUnit.ServerAddressManager.Instance!.SetAddresses("http://localhost:5163");
                                                                                                 //End of TODO
            await Shell.Current.GoToAsync(nameof(ScreenPlacementSelectingPage));
        }

        public MainPage()
        {
            InitializeComponent();
            m_GameInformation.init();
            m_LogicManager = new LogicManager();
            addComponents();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            //addComponents();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnSizeAllocated(double i_Width, double i_Height)
        {
            base.OnSizeAllocated(i_Width, i_Height);
            m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height = (int)i_Height;
            m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width = (int)i_Width;
        }

        protected override void OnAppearing()// works on App() constructor , App OnStart(), MainPage() constructor  
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => await DelayedShowWidth());
        }
        private async Task DelayedShowWidth()
        {
            await Task.Delay(500);
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            m_GameInformation.ScreenDensity =mainDisplayInfo.Density;
            // do something
            // addComponents();
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
            createButtonImage("SKIP:)", OnSkipClicked, 0, 0);
            createButtonImage("Create a Room", OnCreateRoomClicked, 1, 1);
            createButtonImage("Join a Room", OnJoinRoomClicked, 1, 2);
        }

        private void createButtonImage(string i_Text, EventHandler m_ClickEvent, int i_Col, int i_Row)
        {
            ButtonImage btn = new ButtonImage();
            double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            btn.WidthRequest = 0.5 * width;
            btn.HeightRequest = 0.2 * height;
            btn.FontSize = 0.3 * 0.2 * height;

            if (i_Text == "SKIP:)")
            {
                btn.WidthRequest = 0.2 * width;
                btn.HeightRequest = 0.1 * height;
                //IsVisible = false;
                btn.FontSize = 0.3 * 0.1 * height;
            }

            btn.Text = i_Text;
            btn.Source = "entrance_btn.PNG";
            btn.GetButton().Clicked += m_ClickEvent;
            buttonComponent.Add(btn.GetImage(), i_Col, i_Row);
            buttonComponent.Add(btn.GetButton(), i_Col, i_Row);
        }
    }
}