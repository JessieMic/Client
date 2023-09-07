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
    using DTOs;

    namespace LogicUnit.Logic.GamePageLogic.Games.Pong
    {
        public class Pong : Game
        {
            private Bat[] m_Players;
            BombItBoard m_BombItBoard = new BombItBoard();
            private Ball m_Ball;


            public Pong(InGameConnectionManager i_GameConnectionManager)
                : base(i_GameConnectionManager)
            {
                m_GameName = "Pong";
                m_MoveType = eMoveType.ClickAndRelease;
                m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.RightAndLeft;
                m_Hearts.m_AmountOfLivesPlayersGetAtStart = 3;
                m_Players = new Bat[r_GameInformation.AmountOfPlayers];
            }

            protected override void SpecialUpdateReceived(SpecialUpdate i_SpecialUpdate)
            {
                if (i_SpecialUpdate.Update == 2)
                {
                    m_Ball.Reset();
                    resetPlayersSize();
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
                    if (r_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].Position.Row == eRowPosition.UpperRow)
                    {
                        base.PlayerLostALife(null, 1);
                        if (r_GameInformation.AmountOfPlayers == 4)
                        {
                            base.PlayerLostALife(null, 2);
                        }
                    }
                    else
                    {
                        if (r_GameInformation.AmountOfPlayers == 4)
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
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            protected override void specialEventInvoked(object i_Sender, int i_eventNumber)
            {
                if (i_eventNumber == -1)
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
                m_Ball.IncreaseVelocityAndUpdatePoistion(i_SpecialUpdate);
                increasePlayerVelocity();
            }

            private void increasePlayerVelocity()
            {
                foreach (var player in m_Players)
                {
                    player.IncreaseVelocity();
                }
            }

            private void resetPlayersSize()
            {
                foreach (var player in m_Players)
                {
                    player.Reset();
                }
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
                for (int i = 1; i <= r_GameInformation.AmountOfPlayers; i++)
                {
                    Bat newPlayer = new Bat(
                        i,
                        m_BoardSizeByGrid.Width,
                        m_BoardSizeByGrid.Height,
                        m_Board);

                    m_GameObjectsToAdd.Add(newPlayer);
                    m_Players[i - 1] = newPlayer;
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

            protected override void gameLoop()
            {
                m_Ball.Reset();
                base.gameLoop();
            }

            protected override void checkForGameStatusUpdate()
            {
                if ((m_Hearts.m_AmountOfPlayersThatAreAlive <= 1 && m_AmountOfPlayers == 2) ||
                    (m_Hearts.m_AmountOfPlayersThatAreAlive <= 2 && m_AmountOfPlayers == 4))//Search for the player that is alive
                {
                    if (m_AmountOfPlayers == 4)
                    {
                        List<string> names = m_Hearts.GetNamesOfPlayersThatAreAlive();
                        if (names.Count == 2)
                        {
                            m_EndGameText = $"{names[0]} and {names[1]} won !!";
                        }
                    }
                    else
                    {
                        m_EndGameText = $"{m_Hearts.GetNameOfPlayerThatIsAlive()} won!!";
                    }

                    m_ScoreBoard.ShowScoreBoard(m_EndGameText, m_PauseMenu);
                    m_GameStatus = eGameStatus.Ended;
                }
            }
        }
    }

}