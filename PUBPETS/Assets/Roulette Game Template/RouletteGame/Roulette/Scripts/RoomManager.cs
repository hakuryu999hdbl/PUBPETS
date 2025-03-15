using UnityEngine.SceneManagement;
using UnityEngine;
//手动修改
using DG.Tweening;
namespace Roulette_Game
{

    public class RoomManager : MonoBehaviour
    {


        public static void ChangeScene(int SceneID)
        {
            // 在加载新场景前停止所有 DOTween 动画并清理
            DOTween.KillAll();
            DOTween.Clear(true);

            ResultManager.totalBet = 0;
            SceneManager.LoadSceneAsync(SceneID);
        }

        public void GoToScene(int SceneID)
        {

            // 在加载新场景前停止所有 DOTween 动画并清理
            DOTween.KillAll();
            DOTween.Clear(true);


            ResultManager.totalBet = 0;
            SceneManager.LoadSceneAsync(SceneID);
        }
    }
}