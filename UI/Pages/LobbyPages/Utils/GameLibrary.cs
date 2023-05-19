namespace UI.Pages.LobbyPages.Utils
{
    class GameLibrary
    {
        private static Game m_Snake = new Game("snake.png", "Snake");
        private static Game m_Pacman = new Game("pacman.png", "Pacman");

        public static Game GetSnakeGame()
        {
            return m_Snake;
        }

        public static Game GetPacmanGame()
        {
            return m_Pacman;
        }
    }
}
