using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages.LobbyPages.Utils
{
    public static class Instructions
    {
        public static string k_SnakeInstruction = $"Use arrow buttons to move the snake." +
            $"{System.Environment.NewLine}The goal is to become as long as possible by eating apples." +
            $"{System.Environment.NewLine}You should avoid running into walls, your own tail, or other snakes' tail.";

        public static string k_PacmanInstruction = $"Use arrow button to control you player." +
            $"{System.Environment.NewLine}If your player is Pacman, your goal is to eat all Pac-dots, while avoiding the ghosts." +
            $"{System.Environment.NewLine}There are some power-foods, which makes the goasts powerless," +
            $"and Pacman can eat them and get extra points." +
            $"{System.Environment.NewLine}If your player is a ghost, your goal is to eat pacman and prevent him from earning points.";
    }
}
