using POC_Client.Logic;
using POC_Client.Objects;

namespace POC_Client.Pages;

public partial class GamePage : ContentPage
{
    private GameInformation m_GameInformation = GameInformation.Instance;
    private GameLibrary m_GameLibrary = new GameLibrary();
    private Game m_Game = null;

    public GamePage()
	{
		InitializeComponent();
        initializeEvents();
        m_Game = m_GameLibrary.CreateAGame(m_GameInformation.m_NameOfGame);
        m_Game.RunGame();
    }

    private void initializeEvents()
    {

    }
}