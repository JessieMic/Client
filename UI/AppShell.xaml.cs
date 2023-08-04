using System.Globalization;
using UI.Pages;
using UI.Pages.LobbyPages;

namespace UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            var cultureInfo = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-GB");
            InitializeComponent();
            Routing.RegisterRoute(nameof(EnterNamePage), typeof(EnterNamePage));
            Routing.RegisterRoute(nameof(EnterRoomCodePage), typeof(EnterRoomCodePage));
            Routing.RegisterRoute(nameof(Lobby), typeof(Lobby));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
            Routing.RegisterRoute(nameof(ScreenPlacementSelectingPage), typeof(ScreenPlacementSelectingPage));
        }
    }
}