using TMPro;
using UnityEngine;

namespace MADD
{
    public class ResultUI : MonoBehaviour
    {
        public void ShowResult(int result)
        {
            gameObject.SetActive(true);
            GetComponentInChildren<TextMeshProUGUI>().text = "Result: " + result.ToString();
        }
    }
}
