using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages.LobbyPages.Utils
{
    public class Game
    {
        private string m_PicUrl;
        private string m_Name;
        private string m_Details;
        private string m_Instructions;

        public Game(string i_Url, string i_Name, string i_Instructions)
        {
            m_PicUrl = i_Url;
            m_Name = i_Name;
            m_Instructions = i_Instructions;
        }

        public string GetPicUrl()
        {
            return m_PicUrl;
        }

        public string GetName()
        {
            return m_Name;
        }

        public string GetDetails()
        {
            return m_Details;
        }

        public string GetInstructions()
        {
            return m_Instructions;
        }

        public void SetInstructions(string i_Instructions)
        {
            m_Instructions = i_Instructions;
        }
    }
}
