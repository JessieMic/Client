using Objects;

namespace UI.Pages.LobbyPages
{
    public class PlayerObjectController
    {
        private ButtonImage m_RemoveButton;
        private string m_PlayerName;
        private Action<string, ButtonImage> m_RemoveClicked;
        //remove action

        public PlayerObjectController(ButtonImage i_RemoveButton, string i_PlayerName,
            Action<string, ButtonImage> i_Action)
        {
            m_PlayerName = i_PlayerName;
            m_RemoveButton = i_RemoveButton;
            m_RemoveClicked = i_Action;

            m_RemoveButton.GetButton().Clicked += OnRemoveClicked;
        }

        public void OnRemoveClicked(object sender, EventArgs e)
        {
            m_RemoveClicked.Invoke(m_PlayerName, m_RemoveButton);
        }
    }
}
