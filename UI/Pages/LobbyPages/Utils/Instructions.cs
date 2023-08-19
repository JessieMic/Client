using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages.LobbyPages.Utils
{
    public static class Instructions
    {
        public static string k_SnakeInstructions = $"Use arrow buttons to move the snake." +
            $"{System.Environment.NewLine}The goal is to become as long as possible by eating apples." +
            $"{System.Environment.NewLine}You should avoid running into walls, your own tail, or other snakes' tail.";

        public static string k_PacmanInstructions = $"Use arrow buttons to control your player." +
            $"{System.Environment.NewLine}The player at place number 1 will be Pacman, the rest of the players will be ghosts." +
            $"{System.Environment.NewLine}The goal of Pacman is to eat all cookies, and the goal of the ghosts is to eat Pacman and prevent him " +
            $"from eating all cookies." +
            $"{System.Environment.NewLine}If Pacman eats a cherry, he can eat the ghosts for a few seconds. " +
            $"Pacman can also win by eating all the ghosts.";
    }
}
