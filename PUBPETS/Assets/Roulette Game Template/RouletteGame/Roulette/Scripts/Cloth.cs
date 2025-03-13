using UnityEngine;
namespace Roulette_Game
{
    public class Cloth : MonoBehaviour
    {
        public static GameObject chipStackPref;

        //手动修改
        public GameObject ChipStackPref;
        private void Awake()
        {
            //chipStackPref = Instantiate(Resources.Load<GameObject>("chipstack"));
            chipStackPref = Instantiate(ChipStackPref);
        }

        public static ChipStack InstanceStack()
        {
            GameObject ob = Instantiate(chipStackPref);

            return ob.GetComponent<ChipStack>();
        }
    }
}
