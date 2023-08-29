using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages.LobbyPages.Utils
{
    public static class Instructions
    {
        //public static string k_SnakeInstructions = $"Use arrow buttons to move the snake." +
        //    $"{System.Environment.NewLine}The goal is to become as long as possible by eating apples." +
        //    $"{System.Environment.NewLine}You should avoid running into walls, your own tail, or other snakes' tail.";

        public static string k_PacmanInstructions = $"Take control of your character using the arrow buttons." +
            $"{Environment.NewLine} The player holding position number 1 assumes the role of Pacman, " +
            $"while the remaining players embody the ghosts." +
            $"{Environment.NewLine}Pacman's mission is to devour all the Pac-dots." +
            $" Meanwhile, the ghosts need to pursue Pac-Man," +
            $" preventing him from feasting on all the Pac-dots." +
            $"{Environment.NewLine}When Pacman eats a cherry, for a brief span," +
            $" Pacman gains the upper hand by being able to devour the ghosts. " +
            $"Pacman has an alternate path to victory – by consuming all the ghosts.";

        public static string k_BombItInstructions = $"Navigate the maze-like arena using arrow buttons, " +
            $"while a dedicated button lets you drop bombs." +
            $"{Environment.NewLine}Your goal is to eliminate opponents by placing bombs to create " +
            $"explosions that clear obstacles and harm rivals." +
            $"{Environment.NewLine}Be cautious, as your own bombs can also be your downfall.";

        public static string k_PongInstructions = $"Control your paddle with arrow buttons." +
            $"{Environment.NewLine}Your objective is to bounce the ball past your opponents' paddles while" +
            $" protecting your own goal." +
            $"{Environment.NewLine}Remember, the player that won't protect his goal from the ball getting " +
            $"inside it will lead to defeat." +
            $"{Environment.NewLine}This game accommodates either 2 or 4 players.";
    }
}
