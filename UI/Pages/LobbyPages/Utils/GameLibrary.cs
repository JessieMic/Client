namespace UI.Pages.LobbyPages.Utils
{
    class GameLibrary
    {
        private static Game m_Snake = new Game("snake_img_new.png", "Snake", Instructions.k_SnakeInstruction);
        private static Game m_Pacman = new Game("pacman_img_new.png", "Pacman", Instructions.k_PacmanInstruction);

        public static Game GetSnakeGame()
        {
            return m_Snake;
        }

        public static Game GetPacmanGame()
        {
            return m_Pacman;
        }

        public static Game GetGameByName(string i_Name)
        {
            if (i_Name == "Snake")
                return m_Snake;

            return m_Pacman;

            // TODO: add more games when created
        }
    }
}
