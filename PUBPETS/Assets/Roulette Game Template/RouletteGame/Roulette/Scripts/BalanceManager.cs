using UnityEngine;
using UnityEngine.UI;
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



            PlayerPrefs.SetFloat("BalanceKey", Balance);
            Debug.Log("轮盘【最开始设置】完成，目前储存的余额数量" + PlayerPrefs.GetFloat("BalanceKey"));
        }

        public static void ChangeBalance(float value)
        {
            Balance += value;
            SceneRoulette.UpdateLocalPlayerText();


            PlayerPrefs.SetFloat("BalanceKey", Balance);
            Debug.Log("轮盘【赌博/押注】完成，目前储存的余额数量" + PlayerPrefs.GetFloat("BalanceKey"));
        }

        public void ResetBalance(float balance)
        {
            Balance = balance;
            SceneRoulette.UpdateLocalPlayerText();


            PlayerPrefs.SetFloat("BalanceKey", Balance);
            Debug.Log("轮盘【重置】完成，目前储存的余额数量" + PlayerPrefs.GetFloat("BalanceKey"));
        }


    }


}