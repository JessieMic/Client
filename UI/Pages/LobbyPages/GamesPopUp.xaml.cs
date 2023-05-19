using CommunityToolkit.Maui.Views;
using UI.Pages.LobbyPages.Utils;

namespace UI.Pages.LobbyPages;

public partial class GamesPopUp : Popup
{
    private Action<Game> m_FuncForGameChosen;

    public GamesPopUp(Action<Game> i_Action)
	{
		InitializeComponent();
        m_FuncForGameChosen = i_Action;

    }

	public void AddGameToComponent(GameCard i_Card)
	{
        GameCardButton gameCardBtn = new GameCardButton(i_Card);

        //gameCardBtn.AddActionToRadioBtn(i_Action);
        GamesComponent.Add(gameCardBtn);
    }

    public void OnOkClicked(object sender, EventArgs e)
	{
        foreach (GameCardButton cardBtn in GamesComponent)
        {
            if (cardBtn.IsButtonChecked())
            {
                //m_FuncForGameChosen.Invoke(cardBtn.GetGameCard());
                m_FuncForGameChosen.Invoke(cardBtn.GetGameCard().GetGame());
                break;
            }
        }

		Close();
	}

    public void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}