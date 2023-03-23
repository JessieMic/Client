namespace POC_Client;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(ScreenPlacementSelectingPage), typeof(ScreenPlacementSelectingPage));
        //Routing.RegisterRoute(nameof(GameRoomPage), typeof(GameRoomPage));
    }
}
