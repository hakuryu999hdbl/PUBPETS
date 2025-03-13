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
        private void Awake()
        {
            _Instance = this;
        }
        void Start()
        {

            //Debug.Log("百家乐【最开始读取】，目前储存的余额数量" + PlayerPrefs.GetFloat("BalanceKey"));
            //SetBalanceAt(PlayerPrefs.GetFloat("BalanceKey"));
            //balanceText.text = balance.ToString("F2");
            //Debug.Log("百家乐【最开始设置】完成，目前储存的余额数量" + balance);

            float storedBalance = PlayerPrefs.GetFloat("BalanceKey", 0); // 默认值为0，如果没有找到键值
            Debug.Log("百家乐【最开始读取】，目前储存的余额数量" + storedBalance);
            SetBalanceAt(storedBalance);
            Debug.Log("百家乐【最开始设置】完成，目前储存的余额数量" + balance);
        }

        public static void ChangeBalance(float value)
        {

            _Instance.balance += value;
            _Instance.balanceText.text = _Instance.balance.ToString("F2");

         
            PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);
            Debug.Log("百家乐【赌博/押注】完成，目前储存的余额数量" + _Instance.balance);
        }

        public void SetBalanceAt(float value)
        {
            //DOTween.To(() => balance, x => balance = x, value, 1).SetEase(Ease.InOutSine).OnUpdate(() => { balanceText.text = balance.ToString("0"); });

            //balanceText.text = balance.ToString("0");
            //
            ////记录余额
            //PlayerPrefs.SetFloat("BalanceKey", _Instance.balance);
            //Debug.Log("百家乐【重置】完成，目前储存的余额数量" + _Instance.balance);

            balance = value; // 立即更新余额
            PlayerPrefs.SetFloat("BalanceKey", balance); // 立即更新存储的余额
            DOTween.To(() => balance, x => balance = x, value, 1).SetEase(Ease.InOutSine).OnUpdate(() => {
                balanceText.text = balance.ToString("0");
            });
            Debug.Log("百家乐【重置】完成，目前储存的余额数量" + balance);

        }

        public static double GetBalance()
        {
            return _Instance.balance;
        }

    }
}

