using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages.LobbyPages.Utils
{
    public static class Instructions
    {
        public static string k_GeneralInstructions = $"General instructions  - Once you've selected a game and pressed the Ready button, you'll be directed to a screen featuring numbered buttons."
                                                     + $"{Environment.NewLine}Please position your phones so that each phone corresponds to a button on the screen."
                                                     + $"{Environment.NewLine}Ensure that all buttons face the same direction,"
                                                     + $" meaning that some players on the opposite will see the numbers upside down."
                                                     + $"{Environment.NewLine}When you're ready, each player should press the button on their screen that corresponds to their phone."
                                                     + $"{Environment.NewLine}Once all the buttons have been selected, you're ready to start playing!";

        public static string k_PacmanInstructions = $"Take control of your character using the arrow buttons."
                                                    + $"{Environment.NewLine} The player holding position number 1 assumes the role of Pacman, "
                                                    + $"while the remaining players embody the ghosts."
                                                    + $"{Environment.NewLine}Pacman's mission is to devour all the Pac-dots."
                                                    + $" Meanwhile, the ghosts need to pursue Pac-Man,"
                                                    + $" preventing him from feasting on all the Pac-dots."
                                                    + $"{Environment.NewLine}When Pacman eats a cherry, for a brief span,"
                                                    + $" Pacman gains the upper hand by being able to devour the ghosts. "
                                                    + $"Pacman has an alternate path to victory – by consuming all the ghosts.\n\n";

        public static string k_BombItInstructions = $"Navigate the maze-like arena using arrow buttons, "
                                                    + $"while a dedicated button lets you drop bombs."
                                                    + $"{Environment.NewLine}Your goal is to eliminate opponents by placing bombs to create "
                                                    + $"explosions that clear obstacles and harm rivals."
                                                    + $"{Environment.NewLine}Be cautious, as your own bombs can also be your downfall."
                                                    + $"{Environment.NewLine}If your bomb hits a mushroom, that same spot will boost your movement speed"
                                                    + $"and slow enemies who cross it.\n\n";
        
        public static string k_PongInstructions = $"Control your paddle with arrow buttons." +
            $"{Environment.NewLine}Your objective is to bounce the ball past your opponents' paddles while" +
            $" protecting your own goal." +
            $"{Environment.NewLine}Every few seconds your paddle will get shorter and move faster." +
            $"{Environment.NewLine}This game accommodates either 2 or 4 players.\n\n";
    }
}
