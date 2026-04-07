using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace Blackjack_Game
{
    public class Chip : MonoBehaviour, IPointerEnterHandler
    {
        public float value;
        public GameObject rim;

        private Vector3 originalPosition;

        private void Awake()
        {
            originalPosition = transform.localPosition;
        }


        // --- 新增冷却变量 ---
        private static float lastGlobalClickTime = 0f;
        private const float clickCooldown = 0.1f; // 100毫秒，兼顾手感与防错


        public void OnClick()
        {


            // 全局连点/多点触控拦截
            // 使用 static 变量是因为玩家可能两只手指同时点两个不同的筹码，这会导致余额扣减混乱
            if (Time.time - lastGlobalClickTime < clickCooldown) return;
            lastGlobalClickTime = Time.time;



            if (!LimitBetPlate.AllowLimit(value))
            {
                transform.DOComplete();
                transform.DOShakePosition(.2f, 4f, 20, 0);
                return;
            }

            if (ResultManager.betsEnabled && BalanceManager.GetBalance() >= value)
            {
                Player.bet += value;
                BalanceManager.ChangeBalance(-value);
                BetHistoryManager._Instance.Add(value);
                ChipManager.AddToStack(StackType.Standard, value);

                PlaySelectAnimation();

                ChipManager.SelectChip(this);
            }
            else
            {
                //transform.DOComplete();
                //transform.DOShakePosition(.2f, 4f, 20, 0);
            }


            //手动修改
            if(GameManager._Instance.mainCamera.GetInteger("ChangeView")==2)
            {
                GameManager._Instance.ChangeViewBack();
            }
        }

        public void PlaySelectAnimation()
        {
            //AudioManager.SoundPlay(0);
            AudioManager_2.SoundPlay(0);//手动SE音频替换


            //点击触发
            //transform.DOKill();
            //transform.localPosition = originalPosition;
            //transform.DOScale(.9f, .8f).SetEase(Ease.OutElastic).From();
        }

        public void Deselected()
        {
            rim.SetActive(false);
        }
        public void Selected()
        {
            rim.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOComplete();
            transform.DOShakePosition(.2f, 2.2f, 10, 0);
        }
    }
}
