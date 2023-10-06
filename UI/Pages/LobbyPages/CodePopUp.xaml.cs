using CommunityToolkit.Maui.Views;
using Objects;

namespace UI.Pages.LobbyPages;

public partial class CodePopUp : Popup
{
    protected GameInformation m_GameInformation = GameInformation.Instance;

    public CodePopUp(Camera.MAUI.BarcodeImage i_BarcodeImage, string i_RoomCode)
	{
		InitializeComponent();

        Size = new Size(
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width),
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height));

        gridLayout.Add(i_BarcodeImage,2,1);
        roomCodeLabel.Text = i_RoomCode;
        addOKButton();
    }

    public CodePopUp(string i_RoomCode)
    {
        InitializeComponent();

        Size = new Size(
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Width),
            0.7 * (m_GameInformation.m_ClientScreenDimension.ScreenSizeInPixels.Height));

        //gridLayout.Add(i_BarcodeImage, 2, 1);
        roomCodeLabel.Text = i_RoomCode;
        addOKButton();
    }

    private void addOKButton()
    {
        ButtonImage okBtn = new ButtonImage();
        okBtn.Source = "lobby_ready_btn.PNG";
        okBtn.HorizontalOptions = LayoutOptions.FillAndExpand;
        okBtn.VerticalOptions = LayoutOptions.FillAndExpand;
        okBtn.GetButton().VerticalOptions = LayoutOptions.FillAndExpand;
        okBtn.GetButton().HorizontalOptions = LayoutOptions.FillAndExpand;
        okBtn.Text = "OK";
        okBtn.GetButton().Clicked += ClosePopUp;
        gridLayout.Add(okBtn.GetImage(), 4, 3);
        gridLayout.Add(okBtn.GetButton(), 4, 3);
    }

    public void AddQRImage(Camera.MAUI.BarcodeImage i_BarcodeImage)
    {
        gridLayout.Add(i_BarcodeImage, 2, 1);
    }

    public void ClosePopUp(object sender, EventArgs e)
    {
        Close();
    }
}