using static System.Net.WebRequestMethods;

namespace LogicUnit
{

    public class ServerContext
    {
        public const string k_BaseAddress = "http://localhost:5163";
        public const string k_CreateNewRoom = "/CreateNewRoom";
        public const string k_JoinRoom = "/JoinRoom";
    }
}