using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows the user to pickup predefined dice and then roll them. When you pick up the dice and

namespace Baccarat_Game
{
    public class RollDrop : MonoBehaviour
    {
        // This list shows what dice are going to be and can be edited via code if you want or in the inspector. 
        [SerializeField] List<GameObject> diceGroup = new List<GameObject>();
        //Height in which the dice will be picked up at. 
        [SerializeField] float pickUpHeight = 2;
        Camera cam;
        public float speed = 40f;
        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
        }
        // Update is called once per frame
        void Update()
        {
            if (canThrow)
            {
                PickupDropBehavior();
            }



            if (isRolling && !isShow)
            {
                Timer += Time.deltaTime;

                if (Timer >= 0.5f)
                {
                    //Debug.Log("显示花旗骰总和");

                    crapsGame.StartCrapsGame();

                    isRolling = false;
                    isShow = true;//显示过一遍了
                    Timer = 0;

                    AudioManager_2.SoundPlay(2);//手动SE音频替换
                }


            }

        }
        void PickupDropBehavior()
        {
            //Sets the dice to face a random direction when rolled.
            if (Input.GetMouseButtonUp(0))
                for (int i = 0; i < diceGroup.Count; i++)
                {
                    diceGroup[i].transform.rotation = Random.rotation;

                    isShow = false;//感觉这里只触发一次
                    canThrow = false;//扔出后不能再扔了
                }
            // When a user holds down the mouse button the dice will move towards the position of the mouse. You can adjust how high the dice will be if you want.
            if (Input.GetMouseButton(0))
            {
                Vector3 target = new Vector3(0, 0, 0);
                RaycastHit hit;
                //float speed = 40f;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    target = hit.point;
                    //print(hit.transform.position);
                    target.y = pickUpHeight;

                    isRolling = false;//正在捏住

                }
                for (int i = 0; i < diceGroup.Count; i++)
                {
                    diceGroup[i].transform.LookAt(target);
                    diceGroup[i].GetComponent<Rigidbody>().velocity = diceGroup[i].transform.forward * speed;
                }
            }
            else
            {
                isRolling = true;
            }
        }



        //手动修改

        bool isRolling = false;//正在投掷出
        bool isShow = true;//数字已经被显示过一次了
        public bool canThrow = true; // 控制是否可以投掷骰子

        float Timer;
        public CrapsGame crapsGame;//显示数字

    }
}
