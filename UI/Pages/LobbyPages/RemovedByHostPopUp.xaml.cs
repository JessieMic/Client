using CommunityToolkit.Maui.Views;

namespace UI.Pages.LobbyPages;

public partial class RemovedByHostPopUp : Popup
{
	private Action m_ActionWhenClosed;

	public RemovedByHostPopUp(Action i_Action)
	{
        InitializeComponent();
		m_ActionWhenClosed = i_Action;
	}

	public void OnOKBtnClicked(object sender, EventArgs e)
	{
        Close();
        m_ActionWhenClosed.Invoke();
    }
}