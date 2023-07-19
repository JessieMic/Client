using static System.Net.WebRequestMethods;

namespace LogicUnit
{

    public class ServerContext
    {
        //public const string k_BaseAddress = "5.29.17.154:5163";
        public const string k_BaseAddress = "http://localhost:5163";
        public const string k_CreateNewRoom = "/CreateNewRoom";
        public const string k_JoinRoom = "/JoinRoom";
        public const string k_AddPlayer = "/AddPlayer";
        public const string k_UpdatePlayers = "/UpdatePlayers";
        public const string k_UpdatePlayersToRemove = "/UpdatePlayersToRemove";
        public const string k_RemovePlayerByHost = "/RemovePlayerByHost";
        public const string k_PlayerLeft = "/PlayerLeft";
        public const string k_GameChosen = "/GameChosen";
        public const string k_UpdateGame = "/UpdateGame";
    }
}