using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages.LobbyPages.Utils
{
    public static class Messages
    {
        public static string k_RemovedByHost = "You were removed by the host.";
        public static string k_HostLeft = $"The host left the room." +
            $"{Environment.NewLine}Join another room or create a new one.";
        public static string k_WaitForPlayers = $"You can't move on to the game." +
                    $"{Environment.NewLine}Wait for other players to join.";
        public static string k_MustChooseGame = "You must choose a game first.";
        public static string k_PongThreePlayers = $"You can't play Pong with 3 players," +
            $"{Environment.NewLine}only with 2 or 4 players.";
    }
}
