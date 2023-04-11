using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Logic;
using POC_Client.Logic.Games;
using POC_Client.Objects.Enums;

namespace POC_Client.Objects
{
    public class GameLibrary
    {
        public Game CreateAGame(eGames i_GameThatWasPicked)
        {
            Game newGame = null;

            switch(i_GameThatWasPicked)
            {
                case eGames.Pacman:
                    newGame = new Pacman();
                    break;
                case eGames.Pong:
                    newGame = new Pong();
                    break;
                case eGames.Snake:
                    newGame = new Snake();
                    break;
                case eGames.Tanks:
                    newGame = new Tanks();
                    break;
            }

            return newGame;
        }
    }
}
