using UnityEngine;
namespace Roulette_Game
{
    public class DontDestructableObj : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}