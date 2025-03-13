using UnityEngine;
using TMPro;
namespace Baccarat_Game
{
    public class ToolTipManager : MonoBehaviour
    {

        public static ToolTipManager Instance;

        public GameObject toolTip;
        public TMP_Text toolTipText;

        private ChipStack target;

        // Use this for initialization
        void Start()
        {
            Instance = this;
        }

        public void SelectTarget(ChipStack target)
        {
            this.target = target;
            if (target != null)
            {
                if (target.GetValue() > 0)
                {
                    toolTip.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
                    toolTip.SetActive(true);
                    toolTipText.text = target.GetValue().ToString("0");
                }
            }
        }

        public void DeselectTarget()
        {
            target = null;
            toolTip.SetActive(false);
        }
    }
}
