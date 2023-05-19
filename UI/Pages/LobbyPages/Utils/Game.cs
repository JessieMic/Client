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
        private string m_Details = "some game details...";
        private string m_Instructions = "some game instructions...";

        public Game(string i_Url, string i_Name)
        {
            m_PicUrl = i_Url;
            m_Name = i_Name;
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
    }
}
