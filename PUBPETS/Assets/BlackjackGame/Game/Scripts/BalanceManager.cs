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
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            float storedBalance = data.balance;

            //float storedBalance = PlayerPrefs.GetFloat("BalanceKey", initialBalance);【Json存档修改】



            //Debug.Log("黑杰克【最开始读取】，目前储存的余额数量: " + storedBalance);
            SetBalance(storedBalance);
            //Debug.Log("黑杰克【最开始设置】完成，目前储存的余额数量: " + balance);


        }

        public static void ChangeBalance(float value)
        {

            if (value > 0 && GameManager.GameActive)//只有游戲中活得籌碼才可以改變生命值
            {
                GameManager.ChangeHealth(-value,true);
            }


            _Instance.balance += value;
            //_Instance.balanceText.text = "<color=yellow>Balance</color> " + _Instance.balance.ToString();
            _Instance.balanceText.text = _Instance.balance.ToString();


            // ✅ 写入 SaveSystem 的存档中
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            data.balance = _Instance.balance;
            SaveManager.SaveGame(data);
            //PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);【Json存档修改】

            //Debug.Log("黑杰克【赌博/押注】完成，目前储存的余额数量" + _Instance.balance);


            if (_Instance.gameManager != null) 
            {
                //结算画面的总收益
                _Instance.gameManager.revenue += value;

            }


        }

        public GameManager gameManager;


        public void SetBalance(float value)
        {

            //DOTween.To(() => _Instance.balance, x => _Instance.balance = x, value, .5f).OnUpdate(() =>
            //{
            //    _Instance.balanceText.text = "<color=yellow>Balance</color> " + _Instance.balance.ToString();
            //});



            _Instance.balance = value;
            //_Instance.balanceText.text = "<color=yellow>Balance</color> " + _Instance.balance.ToString();
            _Instance.balanceText.text = _Instance.balance.ToString();

            // ✅ 写入存档中
            SaveData data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            data.balance = _Instance.balance;
            SaveManager.SaveGame(data);
            //PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);【Json存档修改】

            //Debug.Log("黑杰克【重置】完成，目前储存的余额数量" + _Instance.balance);
        }


        public static double GetBalance()
        {
            return _Instance.balance;
        }



    }
}
