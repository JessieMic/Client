using CommunityToolkit.Maui.Views;
using Objects;

namespace UI.Pages.LobbyPages;

public partial class MessagePopUp : Popup
{
	private Action m_ActionWhenClosed;
	private string m_Message;
    protected GameInformation m_GameInformation = GameInformation.Instance;

    public MessagePopUp(Action i_Action, string i_Message)
	{
		InitializeComponent();
		m_ActionWhenClosed = i_Action;
		m_Message = i_Message;
		MessageLabel.Text = m_Message;
        ButtonImage okBtn = addOKButton();
		okBtn.GetButton().Clicked += OnOKBtnClicked;
        Size = new Size(
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width ),
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height));
    }

	public MessagePopUp(string i_Message)
	{
        InitializeComponent();
        m_Message = i_Message;
        MessageLabel.Text = m_Message;

        ButtonImage okBtn = addOKButton();
		okBtn.GetButton().Clicked += ClosePopUp;
        Size = new Size(
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width),
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height));
    }

	public void OnOKBtnClicked(object sender, EventArgs e)
	{
		Close();
		m_ActionWhenClosed.Invoke();
	}

    [Obsolete]
    private ButtonImage addOKButton()
	{
		ButtonImage okBtn = new ButtonImage();
		okBtn.Source = "lobby_ready_btn.PNG";
        okBtn.HorizontalOptions = LayoutOptions.CenterAndExpand;
        okBtn.VerticalOptions = LayoutOptions.CenterAndExpand;
        okBtn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
        okBtn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;
		okBtn.Text = "OK";
		gridLayout.Add(okBtn.GetImage(), 2, 2);
		gridLayout.Add(okBtn.GetButton(), 2, 2);

		return okBtn;
    }

	public void ClosePopUp(object sender, EventArgs e)
	{
		Close();
	}
}