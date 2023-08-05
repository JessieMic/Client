using static System.Net.WebRequestMethods;

namespace LogicUnit
{

    public class ServerContext
    {
        //public const string k_BaseAddress = "5.29.17.154:5163";

        //public const string k_BaseAddress = "http://localhost:5163";
        public static string k_BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5163";

        //public const string k_BaseAddress = "https://gameroomserverfinalproject.azurewebsites.net/GameHub";//"http://localhost:5163";
        public const string k_CreateNewRoom = "/CreateNewRoom";
        public const string k_JoinRoom = "/JoinRoom";
        public const string k_AddPlayer = "/AddPlayer";
        public const string k_UpdatePlayers = "/UpdatePlayers";
        public const string k_UpdatePlayersToRemove = "/UpdatePlayersToRemove";
        public const string k_RemovePlayerByHost = "/RemovePlayerByHost";
        public const string k_PlayerLeft = "/PlayerLeft";
        public const string k_GameChosen = "/GameChosen";
        public const string k_UpdateGame = "/UpdateGame";
        public const string k_HostLeft = "/HostLeft";
        public const string k_CheckHostLeft = "/CheckHostLeft";
        public const string k_UpdateGoToNextPage = "/UpdateGoToNextPage";
        public const string k_CheckIfGoToNextPage = "/CheckIfGoToNextPage";
    }
}