using UnityEngine.SceneManagement;
using UnityEngine;
namespace Roulette_Game
{
    public class RoomManager : MonoBehaviour
    {
        public static void ChangeScene(int SceneID)
        {
            ResultManager.totalBet = 0;
            SceneManager.LoadSceneAsync(SceneID);
        }

        public void GoToScene(int SceneID)
        {
            ResultManager.totalBet = 0;
            SceneManager.LoadSceneAsync(SceneID);
        }
    }
}