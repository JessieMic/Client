namespace UI.Pages.LobbyPages;

public partial class PlayerCardHost : ContentView
{
	private Lobby m_ParentLobby;

	public PlayerCardHost(Lobby i_ParentLobby)
	{
		InitializeComponent();
		m_ParentLobby = i_ParentLobby;
	}

	public void ChangePlayerName(string i_PlayerName)
	{
		//PlayerNameLabel.Text = i_PlayerName;
	}

	public void RemoveBtnClicked(object sender, EventArgs e)
	{
		//m_ParentLobby.RemovePlayer(this, PlayerNameLabel.Text);
	}

	public bool CheckIfName(string i_PlayerName)
	{
		return true;//PlayerNameLabel.Text == i_PlayerName;
	}


}