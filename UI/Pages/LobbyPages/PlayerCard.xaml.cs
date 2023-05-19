namespace UI.Pages.LobbyPages;

public partial class PlayerCard : ContentView
{
	Button m_RemoveBtn;
    //private Lobby m_ParentLobby;
    private Action<PlayerCard, string> m_RemoveAction;

    public PlayerCard(Action<PlayerCard, string> i_RemoveAction, string i_PlayerName)
	{
		InitializeComponent();
        //m_ParentLobby = i_ParentLobby;
        m_RemoveAction = i_RemoveAction;
        PlayerNameLabel.Text = i_PlayerName;
    }

    public void AddRemoveButton()
	{
		m_RemoveBtn = new Button();
		m_RemoveBtn.Text = "Remove";
        m_RemoveBtn.Clicked += RemoveBtnClicked;
        m_RemoveBtn.HorizontalOptions = LayoutOptions.Center;
        StackLayout.Add(m_RemoveBtn);
	}

    public void RemoveBtnClicked(object sender, EventArgs e)
    {
        //m_ParentLobby.RemovePlayer(this, PlayerNameLabel.Text);
        m_RemoveAction.Invoke(this, PlayerNameLabel.Text);
    }

    public bool CheckIfName(string i_PlayerName)
    {
        return PlayerNameLabel.Text == i_PlayerName;
    }
}