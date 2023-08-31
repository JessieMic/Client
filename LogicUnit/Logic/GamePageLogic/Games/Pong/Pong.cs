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
            private Bat[] m_BombItPlayers;
            BombItBoard m_BombItBoard = new BombItBoard();
            private Ball m_Ball;
            private int m_FoodCounterForPlayerScreen = 0;
            private int m_AmountOfScreenThatHaveNoFood = 0;
            private int j = 0;

            public Pong()
            {
                m_GameName = "Pong";
                m_MoveType = eMoveType.ClickAndRelease;
                m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.RightAndLeft;
                m_Hearts.m_AmountOfLivesPlayersGetAtStart = 2;
                m_BombItPlayers = new Bat[m_GameInformation.AmountOfPlayers];
            }

            protected override void SpecialUpdateReceived(SpecialUpdate i_SpecialUpdate)
            {
                if (i_SpecialUpdate.Update == 2 )
                {
                    m_Ball.Reset();
                    SideGotHit(i_SpecialUpdate.Player_ID);
                }
                else
                {
                    base.SpecialUpdateReceived(i_SpecialUpdate);
                }
            }

            protected void SideGotHit(int i_Player)
            {
                try
                {
                    if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].Position.Row == eRowPosition.UpperRow)
                    {
                        base.PlayerLostALife(null, 1);
                        if (m_GameInformation.AmountOfPlayers == 4)
                        {
                            base.PlayerLostALife(null, 2);
                        }
                    }
                    else
                    {
                        if (m_GameInformation.AmountOfPlayers == 4)
                        {
                            base.PlayerLostALife(null, 3);
                            base.PlayerLostALife(null, 4);
                        }
                        else
                        {
                            base.PlayerLostALife(null, 2);
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            protected override void specialEventInvoked(object i_Sender, int i_eventNumber)
            {
                if(i_eventNumber == -1)
                {
                    SendServerSpecialPointUpdate(m_Ball.GetCurrentPointOnScreen(), -1);
                }
                else
                {
                    SendSpecialServerUpdate(i_Sender, i_eventNumber);
                }
            }

            protected override void SpecialUpdateWithPointReceived(SpecialUpdate i_SpecialUpdate)
            {
                m_Ball.UpdatePointOnScreenByPixel(m_Ball.GetScreenPoint(new Point(i_SpecialUpdate.X, i_SpecialUpdate.Y),false));
            }

            protected override void AddGameObjects()
            {
                addBall();
                addPlayerObjects();
            }

            private void addBall()
            {
                m_Ball = new Ball();
                m_GameObjectsToAdd.Add(m_Ball);
            }

            private void addPlayerObjects()
            {
                for (int i = 1; i <= m_GameInformation.AmountOfPlayers; i++)
                {
                    Bat newPlayer = new Bat(
                        i,
                        m_BoardSizeByGrid.Width,
                        m_BoardSizeByGrid.Height,
                        m_Board);

                    m_GameObjectsToAdd.Add(newPlayer);
                    m_BombItPlayers[i - 1] = newPlayer;
                }
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
                            m_EndGameText = "Everyone lost!!";
                        }
                        else
                        {
                            string nameOfWinner = m_Hearts.GetNameOfPlayerThatIsAlive();
                            m_EndGameText = $"{nameOfWinner} won!!";
                        }

                        m_ScoreBoard.ShowScoreBoard(m_EndGameText, m_PauseMenu);
                        OnAddScreenObjects();
                        m_GameStatus = eGameStatus.Ended;
                    }
                }
            }
        }
    }

}
