using Objects;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace LogicUnit
{
    // All the code in this file is included in all platforms.
    public class LogicManager
    {
        private readonly Connection r_Connection;
        private Uri m_Uri;
        private HttpClient m_HttpClient = new HttpClient();
        private RoomData m_RoomData;
        private Action<List<string>> m_AddPlayersToScreen;

        ///////////////
        public GameInformation m_GameInformation = GameInformation.Instance;
        public Player m_Player = Player.Instance;

        public LogicManager()
        {
            r_Connection = new Connection();
            //m_GameInformation.m_NameOfGame = Objects.Enums.eGames.Snake;
            m_GameInformation.AmountOfPlayers = 4;
        }

        public string GetRoomCode()
        {
            return m_RoomData.roomCode;
        }

        public async Task<eLoginErrors> CreateNewRoom(string i_HostName)
        {
            if (checkIfValidUsername(i_HostName))
            {
                m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_CreateNewRoom}");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,  m_Uri);
                HttpResponseMessage response = await m_HttpClient.SendAsync(request);
                string s = await response.Content.ReadAsStringAsync();
                m_RoomData = (RoomData)JsonSerializer.Deserialize(s,typeof(RoomData));

                return eLoginErrors.Ok;
            }
            else
            {
                return eLoginErrors.InvalidName;
            }
        }

        public async Task<eLoginErrors> CheckIfValidCode(string i_Code)
        {
            if (i_Code.Length > 0)
            {
                StringContent stringContent = new StringContent($"\"{i_Code}\"", Encoding.UTF8, "application/json");

                m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_JoinRoom}");
                HttpResponseMessage response = await m_HttpClient.PutAsync(m_Uri, stringContent);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return eLoginErrors.CodeNotFound;
                }

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
                //need to send 2 parameters : username, code

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

        public void RemovePlayerByHost(/*string i_Code,*/ string i_PlayerName)
        {
            string code = m_Player.RoomCode;
            // remove in server
        }

        public void PlayerLeft(/*string i_Code, string i_PlayerName*/)
        {
            string code = m_Player.RoomCode;
            string name = m_Player.Name;
            // remove in server
        }

        public void SetAddPlayersAction(Action<List<string>> i_Action)
        {
            m_AddPlayersToScreen = i_Action;
        }

        public void AddPlayersRefresher()
        {
            // invoke m_AddPlayersToScreen with a list of players given from the server
        }
    }
}