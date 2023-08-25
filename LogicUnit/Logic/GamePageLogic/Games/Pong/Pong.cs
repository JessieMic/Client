using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit.Logic.GamePageLogic.Games.Pong
{
    using Objects.Enums;
    using Point = Objects.Point;
    using Objects;
    using Objects.Enums.BoardEnum;
    using System.Numerics;
    using LogicUnit.Logic.GamePageLogic.Games.Pong;
    using Microsoft.AspNetCore.SignalR.Client;
    using global::LogicUnit.Logic.GamePageLogic.Games.BombIt;

    namespace LogicUnit.Logic.GamePageLogic.Games.Pong
    {
        public class Pong : Game
        {
            private BombItPlayer[] m_BombItPlayers;
            BombItBoard m_BombItBoard = new BombItBoard();
            private Ball m_Ball = new Ball();
            private int m_FoodCounterForPlayerScreen = 0;
            private int m_AmountOfScreenThatHaveNoFood = 0;
            private int j = 0;

            public Pong()
            {
                m_GameName = "Pong";
                m_MoveType = eMoveType.ClickAndRelease;
                m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.RightAndLeft;
                m_Hearts.m_AmountOfLivesPlayersGetAtStart = 2;
                m_BombItPlayers = new BombItPlayer[m_GameInformation.AmountOfPlayers];
            }


            protected override void specialEventInvoked(object i_Sender, int i_eventNumber)
            {
                if (i_eventNumber == -1)
                {
                    base.specialEventInvoked(i_Sender, i_eventNumber);
                }
                else
                {
                    //m_GameObjectsToAdd.AddRange(m_BombItPlayers[i_eventNumber - 1].GetExplosions());
                }

            }


            protected override void SpecialUpdateReceived(SpecialUpdate i_SpecialUpdate)
            {
                if (i_SpecialUpdate.Update < 4)
                {
                    PlayerGothit(i_SpecialUpdate.Player_ID);
                }
                else
                {
                    base.SpecialUpdateReceived(i_SpecialUpdate);
                }
            }

            protected override void SpecialUpdateWithPointReceived(SpecialUpdate i_SpecialUpdate)
            {
                //addGameObjectImmediately(m_BombItPlayers[i_SpecialUpdate.Player_ID - 1].PlaceBomb(new Point(i_SpecialUpdate.X, i_SpecialUpdate.Y)));
            }

            private void PlayerGothit(int i_Player)
            {
                double startTimeOfDeathAnimation = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;

                m_BombItPlayers[i_Player - 1].AmountOfLives--;
                m_BombItPlayers[i_Player - 1].DeathAnimation(startTimeOfDeathAnimation);

                base.PlayerLostALife(null, i_Player);
            }

            protected override void AddGameObjects()
            {
                addPlayerObjects();
            }

            private void addPlayerObjects()
            {
                for (int i = 1; i <= m_GameInformation.AmountOfPlayers; i++)
                {
                    BombItPlayer newPlayer = new BombItPlayer(
                        i,
                        m_BoardSizeByGrid.Width,
                        m_BoardSizeByGrid.Height,
                        m_Board);

                    m_GameObjectsToAdd.Add(newPlayer);
                    m_BombItPlayers[i - 1] = newPlayer;
                }

                m_GameObjectsToAdd.Add(m_Ball);
            }

            protected override void UpdatePosition(double i_TimeElapsed)
            {
                m_Ball.Update(i_TimeElapsed);
                base.UpdatePosition(i_TimeElapsed);
            }

            protected override void Draw()
            {
                m_Ball.OnDraw();
                base.Draw();
            }

            protected override void checkForGameStatusUpdate()
            {
                if (m_Hearts.m_AmountOfPlayersThatAreAlive <= 1)//Search for the player that is alive
                {
                    j++;
                    if (j == 5 && m_GameStatus != eGameStatus.Ended)
                    {
                        if (m_Hearts.m_AmountOfPlayersThatAreAlive == 0)
                        {
                            msg = "Everyone lost!!";
                        }
                        else
                        {
                            string nameOfWinner = m_Hearts.GetNameOfPlayerThatIsAlive();
                            msg = $"{nameOfWinner} won!!";
                        }

                        m_scoreBoard.ShowScoreBoard(msg, m_PauseMenu);
                        m_GameObjectsToAdd.Add(m_scoreBoard.ShowScoreBoard(msg, m_PauseMenu));
                        OnAddScreenObjects();
                        m_GameStatus = eGameStatus.Ended;
                    }
                }
            }
        }
    }

}
