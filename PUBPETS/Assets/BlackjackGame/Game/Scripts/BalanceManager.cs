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

        private void Awake()
        {
            _Instance = this;
        }

        void Start()
        {
            float storedBalance = PlayerPrefs.GetFloat("BalanceKey", initialBalance);
            Debug.Log("黑杰克【最开始读取】，目前储存的余额数量" + storedBalance);
            SetBalance(storedBalance);
            Debug.Log("黑杰克【最开始设置】完成，目前储存的余额数量" + balance);
        }

        public static void ChangeBalance(float value)
        {

            if (value>0&& GameManager.GameActive)//只有游戲中活得籌碼才可以改變生命值
            {
                GameManager.ChangeHealth(-value);
            }
           

            _Instance.balance += value;
            //_Instance.balanceText.text = "<color=yellow>Balance</color> " + _Instance.balance.ToString();
            _Instance.balanceText.text = _Instance.balance.ToString();


            PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);
            Debug.Log("黑杰克【赌博/押注】完成，目前储存的余额数量" + _Instance.balance);


        }


        public void SetBalance(float value)
        {

            //DOTween.To(() => _Instance.balance, x => _Instance.balance = x, value, .5f).OnUpdate(() =>
            //{
            //    _Instance.balanceText.text = "<color=yellow>Balance</color> " + _Instance.balance.ToString();
            //});



            _Instance.balance = value;
            //_Instance.balanceText.text = "<color=yellow>Balance</color> " + _Instance.balance.ToString();
            _Instance.balanceText.text = _Instance.balance.ToString();

            PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);
            Debug.Log("黑杰克【重置】完成，目前储存的余额数量" + _Instance.balance);
        }


        public static double GetBalance()
        {
            return _Instance.balance;
        }



    }
}
