namespace Baccarat_Game
{
    public class GameResult
    {
        private int playerResult;
        private int bankerResult;

        public GameResult(int player, int banker)
        {
            playerResult = player;
            bankerResult = banker;
        }

        public void UpdateResult(int player, int banker)
        {
            playerResult = player;
            bankerResult = banker;
        }

        public bool IsTie()
        {
            return playerResult == bankerResult;
        }

        public bool PlayerWon()
        {
            return playerResult > bankerResult;
        }
    }
}