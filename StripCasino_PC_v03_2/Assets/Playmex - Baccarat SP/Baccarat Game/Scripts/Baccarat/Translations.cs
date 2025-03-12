using UnityEngine;
namespace Baccarat_Game
{
    //这里改成了静态，但是百家乐UIStates那边还是报错，我只能注销了
    //public class Translations : MonoBehaviour
    //{
    //    public  string WINNINGS = "Winnings: ";
    //    public  string YOU_LOSE = "You Lose!";
    //    public  string PUSH = "Push";
    //    public  string CREDITS = "CREDITS: ";
    //    public  string BUST = "Bust";
    //    public  string BLACKJACK = "Blackjack";
    //    public  string INSURANCE = "Would you like insurance?";
    //    public  string EVEN_MONEY = "Would you like even money?";
    //    public  string YES = "Yes";
    //    public  string NO = "No";
    //    public  string DEAL = "Deal";
    //    public  string HIT = "Hit";
    //    public  string CLEAR = "Clear";
    //    public  string STAND = "Stand";
    //    public  string DOUBLE_DOWN = "Double Down";
    //    public  string SPLIT = "Split";
    //    public  string SURRENDER = "Surrender";
    //}



    public static class Translations
    {
        public static string[] WINNINGS = { "<color=#ffff00ff>WIN  </color>", "<color=#ffff00ff>GANANCIA </color>" };
        public static string[] YOU_LOSE = { "You Lose!", "Perdiste: " };
        public static string[] PUSH = { "Push", "Empate" };
        public static string[] CREDITS = { "<color=#ffff00ff>BALANCE  </color>", "<color=#ffff00ff>BALANCE  </color>" };
        public static string[] BUST = { "Bust", "Quebrado" };
        public static string[] INSURANCE = { "Would you like insurance?", "¿Te gustaría asegurar?" };
        public static string[] EVEN_MONEY = { "Would you like even money?", "¿Quieres una ganancia pareja?" };
        public static string[] YES = { "Yes", "Sí" };
        public static string[] NO = { "No", "No" };
        public static string[] DEAL = { "Deal", "Repartir" };
        public static string[] HIT = { "Hit", "Pedir" };
        public static string[] CLEAR = { "Change Bet", "Limpiar Apuesta" };
        public static string[] STAND = { "Stand", "Quedarse" };
        public static string[] DOUBLE_DOWN = { "Double Down", "Doblar" };
        public static string[] SPLIT = { "Split", "Dividir" };
        public static string[] REQUEST_HIT = { "Are you sure? You score is high already", "¿Seguro? El puntaje es alto" };
        public static string[] WIN = { "You Win!", "¡Ganaste!" };
        public static string[] REBET = { "Rebet", "Re-apostar" };
        public static string[] REBETx2 = { "Rebet x2", "Re-apostar x2" };
    }

}