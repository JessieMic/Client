using Objects;
using Objects.Enums;
using System.Reflection.Emit;
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
        //private Action<List<string>> m_AddPlayersToScreen;
        private Func<List<string>, bool> m_AddPlayersToScreen;
        private Action<List<string>> m_RemovePlayersByHost;
        private Action<string> m_ChosenGameAction;
        ///////////////
        private GameInformation m_GameInformation = GameInformation.Instance;
        public Player m_Player = Player.Instance;
        private Timer m_TimerForPlayersUpdate;
        private List<string> m_OldPlayersList = new List<string>();
        private string? m_LastChosenGame = null;

        public LogicManager()
        {
            r_Connection = new Connection();

            m_GameInformation.m_NameOfGame = Objects.Enums.eGames.Snake;
            m_GameInformation.AmountOfPlayers = 2;
            m_Player.Name = DateTime.Now.ToString();
            //m_TimerForPlayersUpdate = new Timer(getPlayers, null, 0, 500);
        }

        public string GetRoomCode()
        {
            return m_RoomData.roomCode;
        }

        public async Task<eLoginErrors> CreateNewRoom(string i_HostName)
        {
            if (checkIfValidUsername(i_HostName))
            {
                StringContent stringContent = new StringContent($"\"{i_HostName}\"", Encoding.UTF8, "application/json");

                m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_CreateNewRoom}");
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);
                string s = await response.Content.ReadAsStringAsync();
                m_RoomData = (RoomData)JsonSerializer.Deserialize(s, typeof(RoomData));
                m_Player.Name = i_HostName;
                m_Player.RoomCode = m_RoomData.roomCode;

                //m_TimerForPlayersUpdate = new Timer(getPlayers, null, 0, 500);

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

        public async Task<eLoginErrors> AddPlayerToRoom(string i_UserName, string i_Code)
        {
            if (checkIfValidUsername(i_UserName))
            {
                StringContent stringContent = new(JsonSerializer.Serialize(new
                {
                    RoomCode = i_Code,
                    Name = i_UserName
                }),
                Encoding.UTF8, "application/json");

                m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_AddPlayer}");
                HttpResponseMessage response = await m_HttpClient.PutAsync(m_Uri, stringContent);
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return eLoginErrors.NameTaken;
                }

                m_Player.Name = i_UserName;
                m_Player.RoomCode = i_Code;
                //m_TimerForPlayersUpdate = new Timer(getPlayers, null, 0, 500);

                return eLoginErrors.Ok;
            }
            else
            {
                return eLoginErrors.InvalidName;
            }
        }

        private void updateClient(object stateInfo)
        {
            getPlayers();
            getPlayersToRemove();
            getChosenGame();
        }

        private async void getChosenGame()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_UpdateGame}");
            HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);
            string strResponse = await response.Content.ReadAsStringAsync();

            if (strResponse.Length != 0 && strResponse != m_LastChosenGame)
            {
                if (strResponse == "Snake")
                {
                    m_GameInformation.NameOfGame = eGames.Snake;
                }
                else if (strResponse == "Pacman")
                {
                    m_GameInformation.NameOfGame = eGames.Pacman;
                }
                // TODO: add more games when created
                
                m_LastChosenGame = strResponse;
                m_ChosenGameAction.Invoke(strResponse);
            }
        }

        public async void UpdateChosenGame(string i_GameName, string i_RoomCode)
        {
            StringContent stringContent = new(JsonSerializer.Serialize(new
            {
                RoomCode = i_RoomCode,
                GameName = i_GameName
            }),
            Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_GameChosen}");
            HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);
        }

        private async void getPlayers()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_UpdatePlayers}");
            HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);

            string strResponse = await response.Content.ReadAsStringAsync();
#nullable enable
            List<string>? newPlayersList = JsonSerializer.Deserialize<List<string>>(strResponse);
#nullable disable

            if (m_Player.Name == "Name1")
            {

            }

            if (newPlayersList != null && !newPlayersList.SequenceEqual(m_OldPlayersList)) // checks if the element in the lists are equal
            {
                bool succeed = m_AddPlayersToScreen.Invoke(newPlayersList);
                if (succeed)
                {
                    m_OldPlayersList = newPlayersList;
                }
            }
        }

        private async void getPlayersToRemove()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_UpdatePlayersToRemove}");
            HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);

            string strResponse = await response.Content.ReadAsStringAsync();
#nullable enable
            List<string>? playersToRemove = JsonSerializer.Deserialize<List<string>>(strResponse);
#nullable disable
            if (playersToRemove.Count > 0)
            {
                if (playersToRemove != null)
                {
                    m_RemovePlayersByHost.Invoke(playersToRemove);
                }
            }
        }

        public async Task<eLoginErrors> RemovePlayerByHost(string i_Code, string i_PlayerName)
        {
            StringContent stringContent = new(JsonSerializer.Serialize(new
            {
                RoomCode = i_Code,
                Name = i_PlayerName
            }),
            Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_RemovePlayerByHost}");
            HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);

            return eLoginErrors.Ok;
        }

        public async void PlayerLeft()
        {
            string code = m_Player.RoomCode;
            string name = m_Player.Name;
            StringContent stringContent = new(JsonSerializer.Serialize(new
            {
                RoomCode = code,
                Name = name
            }),
            Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_PlayerLeft}");
            HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);
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

        public void SetAddPlayersAction(Func<List<string>, bool> i_Action)
        {
            m_AddPlayersToScreen = i_Action;
        }

        public void SetPlayersToRemoveAction(Action<List<string>> i_Action)
        {
            m_RemovePlayersByHost = i_Action;
        }

        public void StartUpdatesRefresher()
        {
            m_TimerForPlayersUpdate = new Timer(updateClient, null, 0, 500);
            //updateClient(null);
        }

        public void SetChosenGameAction(Action<string> i_Action)
        {
            m_ChosenGameAction = i_Action;
        }
    }
}