using UnityEngine.SceneManagement;
using UnityEngine;
//手动修改
using DG.Tweening;
namespace Roulette_Game
{

    public class RoomManager : MonoBehaviour
    {
        //为什么放在这里，因为这个是每个场景都有的脚本
        public void Start()
        {
            //Debug.Log("【综合】目前储存的余额数量" + PlayerPrefs.GetFloat("BalanceKey"));
            Debug.Log("【综合】目前储存的语言" + PlayerPrefs.GetFloat("language"));
        }





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