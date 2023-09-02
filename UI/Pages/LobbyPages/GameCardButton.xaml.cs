namespace UI.Pages.LobbyPages;

public partial class GameCardButton : ContentView
{
    private GameCard m_GameCard;

    public GameCardButton(GameCard i_Card)
	{
		InitializeComponent();
        GameButtonComponent.Add(i_Card);
        m_GameCard = i_Card;
    }

	public bool IsButtonChecked()
	{
		return RadioBtn.IsChecked;
	}

	public GameCard GetGameCard()
    {
        return m_GameCard;
    }
}