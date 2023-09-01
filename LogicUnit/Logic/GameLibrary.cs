using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic;
using LogicUnit.Logic.GamePageLogic.Games.BombIt;
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using LogicUnit.Logic.GamePageLogic.Games.Pong.LogicUnit.Logic.GamePageLogic.Games.Pong;
using Objects.Enums;

namespace LogicUnit
{
    public class GameLibrary
    {
        

        public Game CreateAGame(eGames i_GameThatWasPicked, InGameConnectionManager i_GameConnectionManager)
        {
            Game newGame = null;

            switch (i_GameThatWasPicked)
            {
                case eGames.Pacman:
                    newGame = new Pacman(i_GameConnectionManager);
                    break;
                case eGames.BombIt:
                    newGame = new BombIt(i_GameConnectionManager);
                    break;
                case eGames.Pong:
                    newGame = new Pong(i_GameConnectionManager);
                    break;
            }

            return newGame;
        }
    }
}
