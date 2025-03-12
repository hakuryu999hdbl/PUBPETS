using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Baccarat_Game
{
    public class BalanceManager : MonoBehaviour
    {

        [SerializeField]
        private float balance = 0;
        public TMP_Text balanceText;

        private static BalanceManager _Instance;

        //手动修改
        void Start()
        {
            _Instance = this;

            //目前储存的余额数量
            Debug.Log("目前储存的余额数量" + PlayerPrefs.GetFloat("BalanceKey"));

            //读取余额(如果为0，赠送1000)
            if (PlayerPrefs.GetFloat("BalanceKey") == 0)
            {
                Debug.Log("因为持有现金为0，修改余额");
                ChangeBalance(1000);
            }
            else
            {
                ChangeBalance(PlayerPrefs.GetFloat("BalanceKey"));
            }

            balanceText.text = balance.ToString("F2");
        }

        public static void ChangeBalance(float value)
        {
            _Instance.balance += value;
            _Instance.balanceText.text = _Instance.balance.ToString("F2");

            //记录余额
            PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);
        }

        public void SetBalanceAt(float value)
        {
            DOTween.To(() => balance, x => balance = x, value, 1).SetEase(Ease.InOutSine).OnUpdate(() => { balanceText.text = balance.ToString("0"); });

            //记录余额
            PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);
        }

        public static double GetBalance()
        {
            return _Instance.balance;
        }

    }
}

