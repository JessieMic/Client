using System.Net.Http.Json;

namespace LogicUnit
{
    // All the code in this file is included in all platforms.
    public class LogicManager
    {
        private readonly Connection r_Connection;
        private HttpClient m_HttpClient = new HttpClient();
        //private Uri m_Uri = new Uri("");

        public LogicManager()
        {
            r_Connection = new Connection();
        }

        public eLoginErrors CreateNewRoom(string i_HostName)
        {
            if (checkIfValidUsername(i_HostName))
            {
                StringContent stringContent = new StringContent(i_HostName);

                // create a new room with host name in server
                //m_HttpClient.PostAsync(m_Uri, stringContent);

                return eLoginErrors.Ok;
            }
            else
            {
                return eLoginErrors.InvalidName;
            }
        }

        public eLoginErrors CheckIfValidCode(string i_Code)
        {
            if (i_Code.Length > 0)
            {
                StringContent stringContent = new StringContent(i_Code);

                //check if valid code in server
                //m_HttpClient.PutAsync(m_Uri, stringContent);

                return eLoginErrors.Ok;
            }
            else
            {
                return eLoginErrors.EmptyCode;
            }
        }

        public eLoginErrors AddPlayerToRoom(string i_UserName, string i_Code)
        {
            if (checkIfValidUsername(i_UserName))
            {
                //check in server if username is taken.
                //if not - create a new player.
                //need to sent 2 parameters : username, code

                return eLoginErrors.Ok;
            }
            else
            {
                return eLoginErrors.InvalidName;
            }
        }

        private bool checkIfValidUsername(string i_UserName)
        {
            if (i_UserName.Length < 2)
            {
                return false;
            }

            foreach (char c in i_UserName)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}