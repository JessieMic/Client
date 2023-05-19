using CommunityToolkit.Maui.Views;

namespace UI.Pages.LobbyPages;

public partial class InstructionsPopUp : Popup
{
	public InstructionsPopUp(string i_GameName, string i_Instructions)
	{
		InitializeComponent();

		HeaderLabel.Text = "Instructions - " + i_GameName;
		InstructionsLabel.Text = i_Instructions;
	}

	public void OnCloseBtnClicked(object sender, EventArgs e)
	{
		Close();
	}
}