namespace UI.Pages.LobbyPages;

public partial class GameCardButton : ContentView
{
	private Action<GameCard> m_FuncForGameChosen;
    private GameCard m_GameCard;

    public GameCardButton(GameCard i_Card)
	{
		InitializeComponent();
        //RadioBtn.Content = i_Card;
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
        //return (GameCard)RadioBtn.Content;
    }

    //public void AddActionToRadioBtn(Action<GameCard> i_Func)
    //{
    //       m_FuncForGameChosen = i_Func;
    //   }

    //public void OnRadioBtnCheckedChanged(object sender, EventArgs e)
    //{
    //	if ((sender as RadioButton).IsChecked)
    //	{
    //		//m_FuncForGameChosen((GameCard)RadioBtn.Content);
    //		m_FuncForGameChosen.Invoke((GameCard)RadioBtn.Content);
    //	}
    //}
}