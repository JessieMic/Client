using CommunityToolkit.Maui.Views;
using Objects;
using UI.Pages.LobbyPages.Utils;

namespace UI.Pages.LobbyPages;

public partial class GamesPopUp : Popup
{
    private Action<Game> m_FuncForGameChosen;
    protected GameInformation m_GameInformation = GameInformation.Instance;

    public GamesPopUp(Action<Game> i_Action)
	{
		InitializeComponent();
        m_FuncForGameChosen = i_Action;
        addButton("OK", OnOkClicked, 2, 2);
        addButton("Cancel", OnCancelClicked, 2, 0);
        Size = new Size(
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width),
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height));
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

    [Obsolete]
    private void addButton(string i_Text, EventHandler i_ClickEvent, int i_Row, int i_Col)
    {
        ButtonImage btn = new ButtonImage();
        btn.Text = i_Text;
        btn.Source = "lobby_ready_btn.PNG";
        btn.GetButton().Clicked += i_ClickEvent;
        btn.HorizontalOptions = LayoutOptions.CenterAndExpand;
        btn.VerticalOptions = LayoutOptions.CenterAndExpand;
        btn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;
        btn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
        gridLayout.Add(btn.GetImage(), i_Col, i_Row);
        gridLayout.Add(btn.GetButton(), i_Col, i_Row);
    }
}