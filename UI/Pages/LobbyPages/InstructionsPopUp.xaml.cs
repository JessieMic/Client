using CommunityToolkit.Maui.Views;
using Objects;

namespace UI.Pages.LobbyPages;

public partial class InstructionsPopUp : Popup
{
    private GameInformation m_GameInformation = GameInformation.Instance;

    public InstructionsPopUp(string i_GameName, string i_Instructions)
	{
        InitializeComponent();

		HeaderLabel.Text = "Instructions - " + i_GameName;
		InstructionsLabel.Text = i_Instructions;
		addCloseButton();
        Size = new Size(
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width),
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height));
    }

	public void OnCloseBtnClicked(object sender, EventArgs e)
	{
		Close();
	}

	private void addCloseButton()
	{
		ButtonImage closeBtn = new ButtonImage();
		closeBtn.Source = "lobby_ready_btn.PNG";
		closeBtn.VerticalOptions = LayoutOptions.CenterAndExpand;
		closeBtn.HorizontalOptions = LayoutOptions.CenterAndExpand;
        closeBtn.GetImage().HorizontalOptions = LayoutOptions.FillAndExpand;
        closeBtn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
		closeBtn.GetButton().Clicked += OnCloseBtnClicked;
		closeBtn.Text = "Close";
        gridLayout.Add(closeBtn.GetImage(), 0, 2);
		gridLayout.Add(closeBtn.GetButton(), 0, 2);
    }
}