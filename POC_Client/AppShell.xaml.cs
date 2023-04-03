using POC_Client.Pages;

namespace POC_Client;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(ScreenPlacementSelectingPage), typeof(ScreenPlacementSelectingPage));
        Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
    }
}
