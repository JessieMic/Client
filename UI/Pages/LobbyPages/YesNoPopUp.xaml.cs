using CommunityToolkit.Maui.Views;
using Objects;

namespace UI.Pages.LobbyPages;

public partial class YesNoPopUp : Popup
{
    private string m_Message;
    private Action m_ClickYesAction;
    private GameInformation m_GameInformation = GameInformation.Instance;

    public YesNoPopUp(string i_Message, Action i_ClickYesAction)
	{
		InitializeComponent();
        m_Message = i_Message;
        m_ClickYesAction = i_ClickYesAction;
        MessageLabel.Text = m_Message;

        createBtnImages();
        Size = new Size(
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width),
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height));
    }

    private void createBtnImages()
    {
        ButtonImage yesBtn = addButton("Yes", 2, 2);
        ButtonImage noBtn = addButton("No", 2, 0);
        yesBtn.GetButton().Clicked += yesBtnClicked;
        noBtn.GetButton().Clicked += noButtonClicked;
    }

    private ButtonImage addButton(string i_Text, int i_Row, int i_Col)
    {
        ButtonImage btn = new ButtonImage();
        btn.Source = "lobby_ready_btn.PNG";
        btn.HorizontalOptions = LayoutOptions.CenterAndExpand;
        btn.VerticalOptions = LayoutOptions.CenterAndExpand;
        btn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
        btn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;
        //btn.GetButton().Clicked += OnOKBtnClicked;
        btn.Text = i_Text;
        gridLayout.Add(btn.GetImage(), i_Col, i_Row);
        gridLayout.Add(btn.GetButton(), i_Col, i_Row);

        return btn;
    }

    private void yesBtnClicked(object sender, EventArgs e)
    {
        Close();
        m_ClickYesAction.Invoke();
    }

    private void noButtonClicked(object sender, EventArgs e)
    { 
        Close();
    }
}