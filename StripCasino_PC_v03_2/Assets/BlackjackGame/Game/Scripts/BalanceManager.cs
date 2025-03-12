using UnityEngine;
using DG.Tweening;
using TMPro;
namespace Blackjack_Game
{
    public class BalanceManager : MonoBehaviour
    {
        //手动修改
        private float balance = 0;

        public float initialBalance = 1000;
        public TMP_Text balanceText;

        private static BalanceManager _Instance;

        void Start()
        {
            _Instance = this;

            //目前储存的余额数量
            Debug.Log("目前储存的余额数量" + PlayerPrefs.GetFloat("BalanceKey"));

            //读取余额(如果为0，赠送1000)
            if (PlayerPrefs.GetFloat("BalanceKey") == 0)
            {
                SetBalance(initialBalance);
            }
            else
            {
                SetBalance(PlayerPrefs.GetFloat("BalanceKey"));
            }

        }

        public static void ChangeBalance(float value)
        {
            _Instance.balance += value;
            _Instance.balanceText.text = "<color=yellow>Balance</color> " + _Instance.balance.ToString();

            //记录余额
            PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);
        }

        //这里是重置位置
        public void SetBalance(float value)
        {
            DOTween.To(() => _Instance.balance, x => _Instance.balance = x, value, .5f).OnUpdate(() =>
            {
                _Instance.balanceText.text = "<color=yellow>Balance</color> " + _Instance.balance.ToString();
            });
            //记录余额
            PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);
        }

        public static double GetBalance()
        {
            return _Instance.balance;
        }



    }
}
