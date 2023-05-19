using UI.Pages.LobbyPages.Utils;

namespace UI.Pages.LobbyPages;

public partial class GameCard : ContentView
{
	private Game m_Game;

	//public GameCard(GameCard i_Card) // copy ctor
	//{
	//	InitializeComponent();
	//	this.GameImage.Source = i_Card.GameImage.Source;
	//	this.GameNameLabel.Text = i_Card.GameNameLabel.Text;
	//}

	public GameCard(string i_Url, string i_Name)
	{
		InitializeComponent();
		GameImage.Source = i_Url;
		GameNameLabel.Text = i_Name;
	}

	public GameCard(Game i_Game)
	{
		InitializeComponent();
		m_Game = i_Game;
		GameImage.Source = i_Game.GetPicUrl();
        GameNameLabel.Text = i_Game.GetName();
    }

	public Game GetGame()
	{
		return m_Game;
	}

}