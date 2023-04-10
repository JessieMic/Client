using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Objects;

namespace POC_Client.Logic
{
    public abstract class Game
    {
        private GameInformation m_GameInformation = GameInformation.Instance;
        private Player m_Player = Player.Instance;
        //protected ScreenMapping m_ScreenMapping = new ScreenMapping();
        protected eTypeOfGameButtons m_TypeOfGameButtons;
        protected int m_AmountOfLivesTheClientHas;
        protected int m_AmountOfActivePlayers;

        public abstract void RunGame();

        void a ()
        {
            ScreenObject button;
            new ScreenObject(eScreenObjectType.Button,eButton.Up,2,1,35,35,string.Empty,null);
            new ScreenObject(eScreenObjectType.Button, eButton.Right, 3, 2, 35, 35, string.Empty, null);
            new ScreenObject(eScreenObjectType.Button, eButton.Left, 1, 2, 35, 35, string.Empty, null);
            new ScreenObject(eScreenObjectType.Button, eButton.Down, 2, 3, 35, 35, string.Empty, null);

            //1
            new ScreenObject(eScreenObjectType.Image, null, 4, 4, 585, 0, "aa.png", null);
            //2
            new ScreenObject(eScreenObjectType.Image, null, 0, 4, 585, 0, "aa.png", null);
            //3
            new ScreenObject(eScreenObjectType.Image, null, 4, 0, 585, 0, "aa.png", null);
            //4
            new ScreenObject(eScreenObjectType.Image, null, 0, 0, 585, 0, "aa.png", null);
        }


        public bool SetAmountOfPlayers(int i_amountOfPlayers)
        {
            if(i_amountOfPlayers >= 2 && i_amountOfPlayers <= 4)
            {
                m_GameInformation.AmountOfPlayers = i_amountOfPlayers;
                m_AmountOfActivePlayers = i_amountOfPlayers;
                return true;
            }
            else
            {
                return false; //If false is returned tell user that the number of players isn't right
            }
        }

        protected eGameStatus PlayerLostALife(string i_NameOfClientThatLostALife)
        {
            eGameStatus returnStatus = eGameStatus.Running;

            m_AmountOfLivesTheClientHas--;

            if (i_NameOfClientThatLostALife == m_Player.Name)
            {
                // Add a function here that tells the UI to take a heart away 
            }

            if (m_AmountOfLivesTheClientHas == 0)
            {
                returnStatus = eGameStatus.Lost;
            }

            return returnStatus;
        }

        protected eGameStatus ClientLostGame(string i_NameOfClientThatLost)
        {
            eGameStatus returnStatus = eGameStatus.Running;

            m_AmountOfActivePlayers--;

            if (i_NameOfClientThatLost == m_Player.Name)
            {
                // Add a function here that tells the UI what to show to the client in case of 
            }

            if(m_AmountOfActivePlayers == 0)
            {
                returnStatus = eGameStatus.Ended;
            }

            return returnStatus;
        }



    }
}
