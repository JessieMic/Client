using CommunityToolkit.Maui.Views;
using Objects;
using System;

namespace UI.Pages.LobbyPages;

public partial class MessagePopUp : Popup
{
	private Action m_ActionWhenClosed;
	private string m_Message;

	public MessagePopUp(Action i_Action, string i_Message)
	{
		InitializeComponent();
		m_ActionWhenClosed = i_Action;
		m_Message = i_Message;
		MessageLabel.Text = m_Message;

        ButtonImage okBtn = addOKButton();
		okBtn.GetButton().Clicked += OnOKBtnClicked;
    }

	public MessagePopUp(string i_Message)
	{
        InitializeComponent();
        //m_ActionWhenClosed = ClosePopUp;
        m_Message = i_Message;
        MessageLabel.Text = m_Message;

        ButtonImage okBtn = addOKButton();
		okBtn.GetButton().Clicked += ClosePopUp;
    }

	public void OnOKBtnClicked(object sender, EventArgs e)
	{
		Close();
		m_ActionWhenClosed.Invoke();
	}

	private ButtonImage addOKButton()
	{
		ButtonImage okBtn = new ButtonImage();
		okBtn.Source = "lobby_ready_btn.PNG";
        okBtn.HorizontalOptions = LayoutOptions.CenterAndExpand;
        okBtn.VerticalOptions = LayoutOptions.CenterAndExpand;
        okBtn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
        okBtn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;
        //okBtn.GetButton().Clicked += OnOKBtnClicked;
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