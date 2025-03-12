using UnityEngine;
using UnityEngine.UI;
namespace Baccarat_Game
{
    public class Chip : MonoBehaviour
    {

        Vector3 startScale = Vector3.one;

        const float SELECTED_SCALE = .9f;

        public float value;

        private Animator anim;
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

            ChipManager.selected.Deselect();
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
    }
}
