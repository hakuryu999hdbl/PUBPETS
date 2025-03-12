using UnityEngine;
using UnityEngine.UI;
namespace Roulette_Game
{
    public class RoomButton : MonoBehaviour
    {
        private void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}