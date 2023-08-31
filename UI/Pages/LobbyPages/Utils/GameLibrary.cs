namespace UI.Pages.LobbyPages.Utils
{
    class GameLibrary
    {
        //private static Game m_Snake = new Game("snake_img_new.png", "Snake", Instructions.k_SnakeInstructions);
        private static Game m_Pacman = new Game("pacman_img_new.png", "Pacman", Instructions.k_PacmanInstructions);
        private static Game m_BombIt = new Game("bomb.png", "Bomb-It", Instructions.k_BombItInstructions);
        private static Game m_Pong = new Game("","Pong", Instructions.k_PongInstructions);
        //public static Game GetSnakeGame()
        //{
        //    return m_Snake;
        //}

        public static Game GetPacmanGame()
        {
            return m_Pacman;
        }

        public static Game GetBombItGame()
        {
            return m_BombIt;
        }

        public static Game GetPongGame()
        {
            return m_Pong;
        }

        public static Game GetGameByName(string i_Name)
        {
            i_Name = i_Name.ToLower();
            //if (i_Name == "Snake")
            //    return m_Snake;

            //return m_Pacman;

            //// TODO: add more games when created
            ///
            switch (i_Name)
            {
                case "pacman":
                    return m_Pacman;
                case "bomb-it":
                    return m_BombIt;
                case "pong":
                    return m_Pong;
            }

            return null;
        }
    }
}
