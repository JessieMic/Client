using UI.Pages.LobbyPages.Utils;

namespace UI.Pages.LobbyPages;

public partial class GameCard : ContentView
{
	private Game m_Game;

	public GameCard(string i_Url, string i_Name)
	{
		InitializeComponent();
		GameNameLabel.Text = i_Name;
	}

	public GameCard(Game i_Game)
	{
		InitializeComponent();
		m_Game = i_Game;
		GameImage.Source = ImageSource.FromFile(i_Game.GetPicUrl());

		if (i_Game == GameLibrary.GetBombItGame())
		{
			GameImage.Source = ImageSource.FromFile("bombit_img_games.png");
        }

        GameNameLabel.Text = i_Game.GetName();
    }

	public Game GetGame()
	{
		return m_Game;
	}

}