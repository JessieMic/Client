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
        private const int k_MinNameLength = 2;
        private const int k_MaxNameLength = 10;
        private const int k_UpdateTimerInterval = 500;

        private readonly Connection r_Connection;
        private Uri m_Uri;
        private HttpClient m_HttpClient = new HttpClient();
        private RoomData m_RoomData;
        //private Action<List<string>> m_AddPlayersToScreen;
        private Func<List<string>, bool> m_AddPlayersToScreen;
        private Func<List<string>, bool> m_RemovePlayersByHost;
        private Action<string> m_ChosenGameAction;
        ///////////////
        private GameInformation m_GameInformation = GameInformation.Instance;
        public Player m_Player = Player.Instance;
        private Timer m_TimerForPlayersUpdate;
        private List<string> m_OldPlayersList = new List<string>();
        private string? m_LastChosenGame = null;
        private Action m_HostLeftAction;
        private Action m_ServerErrorAction;
        private Action m_GoToNextPage;

        public LogicManager()
        {
            r_Connection = new Connection();

            m_GameInformation.m_NameOfGame = Objects.Enums.eGames.Snake;
            //m_GameInformation.AmountOfPlayers = 2;
            m_GameInformation.AmountOfPlayers = 0;
            //m_Player.Name = DateTime.Now.ToString();
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
                HttpResponseMessage response;

                m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_CreateNewRoom}");
                try
                {
                    response = await m_HttpClient.PostAsync(m_Uri, stringContent);
                    string s = await response.Content.ReadAsStringAsync();
                    m_RoomData = JsonSerializer.Deserialize<RoomData>(s);
                    m_Player.Name = i_HostName;
                    m_Player.RoomCode = m_RoomData.roomCode;
                }
                catch(Exception e)
                {
                    return eLoginErrors.ServerError;
                }

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

                try
                {
                    HttpResponseMessage response = await m_HttpClient.PutAsync(m_Uri, stringContent);
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return eLoginErrors.CodeNotFound;
                    }
                    else if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        return eLoginErrors.FullRoom;
                    }
                }
                catch(Exception e)
                {
                    return eLoginErrors.ServerError;
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

                try
                {
                    HttpResponseMessage response = await m_HttpClient.PutAsync(m_Uri, stringContent);
                    if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        return eLoginErrors.NameTaken;
                    }

                    m_Player.Name = i_UserName;
                    m_Player.RoomCode = i_Code;
                    //m_TimerForPlayersUpdate = new Timer(getPlayers, null, 0, 500);
                    m_GameInformation.AmountOfPlayers++;
                    return eLoginErrors.Ok;
                }
                catch(Exception e)
                {
                    return eLoginErrors.ServerError;
                }
            }
            else
            {
                return eLoginErrors.InvalidName;
            }
        }

        private void updateClient(object stateInfo)
        {
            getPlayers();
            //getPlayersToRemove();
            getChosenGame();
            getIfHostLeft();
            checkIfNeedToGoToNextPage();
        }

        private async void getChosenGame()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_UpdateGame}");
            try
            {
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
            catch (Exception e)
            {
                m_ServerErrorAction.Invoke();
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

            try
            {
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);
            }
            catch(Exception e)
            {
                m_ServerErrorAction.Invoke();
            }
        }

        private async void getPlayers()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_UpdatePlayers}");

            try
            {
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);

                string strResponse = await response.Content.ReadAsStringAsync();
#nullable enable
                List<string>? newPlayersList = JsonSerializer.Deserialize<List<string>>(strResponse);
#nullable disable

                if (newPlayersList != null && !newPlayersList.SequenceEqual(m_OldPlayersList)) // checks if the element in the lists are equal
                {
                    bool succeed = m_AddPlayersToScreen.Invoke(newPlayersList);
                    if (succeed)
                        m_OldPlayersList = newPlayersList;
                    m_GameInformation.AmountOfPlayers = newPlayersList.Count;
                }
            }
            catch(Exception e)
            {
                m_ServerErrorAction.Invoke();
            }
        }

        private async void getPlayersToRemove()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");

            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_UpdatePlayersToRemove}");
            HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);

            string strResponse = await response.Content.ReadAsStringAsync();
            if (strResponse != null && strResponse.Length > 0)
            {
#nullable enable
                List<string>? playersToRemove = JsonSerializer.Deserialize<List<string>>(strResponse);
#nullable disable

                bool succeed = false;

                if (playersToRemove.Count > 0)
                {
                    if (playersToRemove != null)
                    {
                        while (!succeed)
                        {
                            if (playersToRemove.Contains(m_Player.Name))
                            {
                                StopUpdatesRefresher();
                                succeed = m_RemovePlayersByHost.Invoke(playersToRemove);
                            }
                        }

                        //m_GameInformation.AmountOfPlayers -= playersToRemove.Count;
                    }
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

            try
            {
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);

                return eLoginErrors.Ok;
            }
            catch (Exception e)
            {
                return eLoginErrors.ServerError;
            }
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

            try
            {
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);
            }
            catch (Exception e)
            {
                m_ServerErrorAction.Invoke();
            }
        }

        public async void HostLeft()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");
            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_HostLeft}");
            
            try
            {
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);
            }
            catch (Exception e)
            {
                m_ServerErrorAction.Invoke();
            }
        }

        private async void getIfHostLeft()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");
            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_CheckHostLeft}");

            try
            {
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);

                string strResponse = await response.Content.ReadAsStringAsync();
                bool hostLeft = JsonSerializer.Deserialize<bool>(strResponse);

                if (hostLeft)
                {
                    m_HostLeftAction.Invoke();
                }
            }
            catch(Exception e)
            {
                m_ServerErrorAction.Invoke();
            }
        }

        public int GetAmountOfPlayers()
        {
            return m_GameInformation.AmountOfPlayers;
        }

        public async void UpdateServerToMoveToNextPage()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");
            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_UpdateGoToNextPage}");

            try
            {
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);
            }
            catch (Exception e)
            {
                m_ServerErrorAction.Invoke();
            }
        }

        private async void checkIfNeedToGoToNextPage()
        {
            StringContent stringContent = new StringContent($"\"{m_Player.RoomCode}\"", Encoding.UTF8, "application/json");
            m_Uri = new Uri($"{ServerContext.k_BaseAddress}{ServerContext.k_CheckIfGoToNextPage}");

            try
            {
                HttpResponseMessage response = await m_HttpClient.PostAsync(m_Uri, stringContent);

                string strResponse = await response.Content.ReadAsStringAsync();
                bool goToNextPage = JsonSerializer.Deserialize<bool>(strResponse);

                if (goToNextPage)
                {
                    m_GoToNextPage.Invoke();
                }
            }
            catch (Exception e)
            {
                m_ServerErrorAction.Invoke();
            }

        }

        private bool checkIfValidUsername(string i_UserName)
        {
            if (i_UserName.Length < k_MinNameLength || i_UserName.Length > k_MaxNameLength)
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

        public void SetPlayersToRemoveAction(Func<List<string>, bool> i_Action)
        {
            m_RemovePlayersByHost = i_Action;
        }

        public void SetHostLeftAction(Action i_Action)
        {
            m_HostLeftAction = i_Action;
        }

        public void StartUpdatesRefresher()
        {
            m_TimerForPlayersUpdate = new Timer(updateClient, null, 0, k_UpdateTimerInterval);
            //updateClient(null);
        }

        public void SetChosenGameAction(Action<string> i_Action)
        {
            m_ChosenGameAction = i_Action;
        }

        public void SetServerErrorAction(Action i_Action)
        {
            m_ServerErrorAction = i_Action;
        }

        public void SetGoToNextPageAction(Action i_Action)
        {
            m_GoToNextPage = i_Action;
        }

        public void StopUpdatesRefresher()
        {
            m_TimerForPlayersUpdate.Dispose();
        }
    }
}