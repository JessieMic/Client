namespace LogicUnit
{

    public class Player
    {
        private string m_Name;
        private ePlayerType m_PlayerType;
        private float m_Width;
        private float m_Height;

        public void SetName(string i_Name)
        {
            m_Name = i_Name;
        }

        public void SetPlayerType(ePlayerType i_PlayerType)
        {
            m_PlayerType = i_PlayerType;
        }

        public void SetWidth(float i_Width)
        {
            m_Width = i_Width;
        }

        public void SetHeight(float i_Height)
        {
            m_Height = i_Height;
        }
    }
}