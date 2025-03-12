using UnityEngine;
namespace Roulette_Game
{
    public class BalanceManager : MonoBehaviour
    {
        //手动修改
        public static float Balance { get; private set; } = 0;

        public static void SetBalance(float balance)
        {
            Balance = balance;
            SceneRoulette.UpdateLocalPlayerText();

            //最开始重置，记录余额
            PlayerPrefs.SetFloat("BalanceKey", Balance);
        }

        public static void ChangeBalance(float value)
        {
            Balance += value;
            SceneRoulette.UpdateLocalPlayerText();

            //记录余额
            PlayerPrefs.SetFloat("BalanceKey", Balance);
        }

        public void ResetBalance(float balance)
        {
            Balance = balance;
            SceneRoulette.UpdateLocalPlayerText();

            //重置后记录余额
            PlayerPrefs.SetFloat("BalanceKey", Balance);
        }
    }
}