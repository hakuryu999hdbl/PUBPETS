using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Baccarat_Game
{
    public class Chip : MonoBehaviour
    {

        Vector3 startScale = Vector3.one;

        const float SELECTED_SCALE = .9f;

        public float value;

        //手动修改
        public Animator anim;
        public GameObject circle;
        private void Awake()
        {
            ResultManager.onResult += TurnUpColor;
            anim = GetComponent<Animator>();
        }

        void Start()
        {
            bool selected = ChipManager.selected.Equals(this);
            anim.SetBool("OnSelected", selected);
            startScale = transform.localScale;
            transform.localScale = selected ? startScale * 1.1f : startScale;
            circle.SetActive(selected);
        }
  

        private void OnDisable()
        {
            ResultManager.onResult -= TurnUpColor;
        }

        public void OnEnter()
        {
            if (ResultManager.betsEnabled)
                gameObject.transform.localScale = startScale * SELECTED_SCALE;
        }

        public void OnExit()
        {
            if (ResultManager.betsEnabled && ChipManager.selected != this)
                gameObject.transform.localScale = startScale;
            else if (ResultManager.betsEnabled)
                gameObject.transform.localScale = startScale * 1.1f;

        }

        public void OnClick()
        {
            if (!ResultManager.betsEnabled)
                return;

            //当第二次进场景的时候，不知为何，取不到别的筹码的动画器，所以算了吧，大家都不要动画效果了
            //ChipManager.selected.Deselect();
            // 取消选择列表中的其他筹码
            foreach (Chip chip in otherChips)
            {
                chip.Deselect();
            }


            ChipManager.selected = this;
            transform.localScale = this.startScale * 1.1f;
            circle.SetActive(true);
            if (anim)
                anim.SetBool("OnSelected", true);
        }

        public void TurnDownColor()
        {
            gameObject.GetComponent<Button>().interactable = false;
            if (anim)
                anim.SetBool("OnSelected", false);
        }
        public void TurnUpColor()
        {
            if (ChipManager.selected == null)
                return;
            gameObject.GetComponent<Button>().interactable = true;
            anim.SetBool("OnSelected", ChipManager.selected.Equals(this));
        }

        public void Deselect()
        {
            anim.SetBool("OnSelected", false);
            circle.SetActive(false);
            gameObject.transform.localScale = this.startScale;
           
        }


        // 可在编辑器中设置的其他筹码列表
        public List<Chip> otherChips;

    }
}
