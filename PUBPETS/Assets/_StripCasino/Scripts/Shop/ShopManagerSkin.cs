using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Spine;
using UnityEngine;
namespace Blackjack_Game
{
    public class ShopManagerSkin : MonoBehaviour
    {
        /// <summary>
        /// 皮肤/初始随机商品
        /// </summary>
        #region
        [Header("皮肤")]
        SkeletonMecanim skeletonAnimation;
        Skin blendSkin = new Skin("BlendedSkin");// 创建一个新的混合皮肤


        // Start is called before the first frame update
        void Awake()
        {
            //换皮肤
            skeletonAnimation = GetComponent<SkeletonMecanim>();

            //初始皮肤
            //ShowCurrentAll(1,2,3,4);

            blendSkin = new Spine.Skin("BlendedSkin");
            RandomizeFour();   // 1) 抽 4 个不重复
            ApplySkins();      // 2) 显示到 Spine

        }
        #region 一起随机
        //public void ShowCurrentAll
        //   (
        //      int Item_1, int Item_2, int Item_3, int Item_4
        //   )
        //{
        //
        //    if (Item_1 != 0) { blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin($"Item_1/Item_{Item_1}")); }
        //    if (Item_2 != 0) { blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin($"Item_2/Item_{Item_2}")); }
        //    if (Item_3 != 0) { blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin($"Item_3/Item_{Item_3}")); }
        //    if (Item_4 != 0) { blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin($"Item_4/Item_{Item_4}")); }
        //
        //    skeletonAnimation.Skeleton.SetSkin(blendSkin);
        //    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        //}
        #endregion

        // 随机 4 个不重复（1..8）
        public void RandomizeFour()
        {
            List<int> pool = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            for (int i = 0; i < 4; i++)
            {
                int r = Random.Range(0, pool.Count);
                current[i] = pool[r];
                pool.RemoveAt(r);
            }
        }
        // 按你现有命名：Item_{槽}/Item_0{编号}
        private void ApplySkins()
        {
            if (skeletonAnimation == null) return;

            blendSkin.Clear();

            if (current[0] != 0) AddSkin("Item_1/Item_" + current[0]);
            if (current[1] != 0) AddSkin("Item_2/Item_" + current[1]);
            if (current[2] != 0) AddSkin("Item_3/Item_" + current[2]);
            if (current[3] != 0) AddSkin("Item_4/Item_" + current[3]);

            skeletonAnimation.Skeleton.SetSkin(blendSkin);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();

            void AddSkin(string path)
            {
                var skin = skeletonAnimation.Skeleton.Data.FindSkin(path);
                if (skin != null) blendSkin.AddSkin(skin);
                else Debug.LogWarning("Skin not found: " + path);
            }
        }

        #endregion


        /// <summary>
        /// 帧事件/鼠标悬停跳出对话框
        /// </summary>
        #region
        [Header("帧事件")]
        public GameObject Item_All;

        public void Open()
        {
            Item_All.SetActive(true);
        }

        [Header("商品介绍配置")]
        public List<GameObject> all_ItemsIntroduce;
        [Header("当前四个物品(1..8)")]
        [SerializeField] private int[] current = new int[4];   // 0..3 槽位



        // —— 鼠标事件供按钮调用 —— //
        public void ShowIntroForSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > 3) return;
            int id = current[slotIndex];  // 1..8
            ShowIntroByItemId(id);
        }//检测物品

        public void ShowIntroByItemId(int id)
        {
            HideAllIntros();
            if (all_ItemsIntroduce == null) return;
            if (id >= 1 && id < all_ItemsIntroduce.Count)
            {
                var go = all_ItemsIntroduce[id];
                if (go != null) go.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Intro index out of range: " + id);
            }
        }//弹出对应物品介绍

        public void HideAllIntros()
        {
            if (all_ItemsIntroduce == null) return;
            foreach (var go in all_ItemsIntroduce)
                if (go) go.SetActive(false);
        }//隐藏全部介绍

        #endregion



        /// <summary>
        /// 购买
        /// </summary>
        #region
        [Header("商品介绍配置")]
        public ItemManager itemManager;//刷新物品

        // 槽位点击购买
        public bool TryBuySlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > 3) return false;
            int id = current[slotIndex];          // 1..8
            if (id <= 0) return false;

            int price = GetPrice(id);
            if (price <= 0) return false;


            // 1) 读取存档余额并判断
            var data = SaveManager.LoadGame(GameFlowData.CurrentPlayer);
            float storedBalance = data.balance;
            if (storedBalance < price)
            {
                // 不够钱：播放失败音效 + 日志，直接返回
                AudioManager_2.SoundPlay(4);
                Debug.Log($"余额不足：当前 {storedBalance}G，需要 {price}G，无法购买 Item_{id}");
                return false;
            }

            // 2) 足够：扣钱（更新UI用）+ 写回存档
            BalanceManager.ChangeBalance(-price); // 你已经在场景放了 BalanceManager

            // 保持存档一致（若 BalanceManager 内部已写存档，这两行可删）
            data.balance = storedBalance - price;

            // 存档里对应物品 +1

            switch (id)
            {
                case 1: data.Item_1 += 1; break;  // 跳蛋
                case 2: data.Item_2 += 1; break;  // 水晶
                case 3: data.Item_3 += 1; break;  // 均衡
                case 4: data.Item_4 += 1; break;  // 宝石
                case 5: data.Item_5 += 1; break;  // 酒瓶
                case 6: data.Item_6 += 1; break;  // 翡翠
                case 7: data.Item_7 += 1; break;  // 硬币
                case 8: data.Item_8 += 1; break;  // 旧酒
            }
            SaveManager.SaveGame(data);

            Debug.Log($"购买成功：Item_{id} 价格 {price}G");

            Debug.Log($"库存｜跳蛋:{data.Item_1}  水晶:{data.Item_2}  法杖:{data.Item_3}  宝石:{data.Item_4}  " + $"酒瓶:{data.Item_5}  翡翠:{data.Item_6}  硬币:{data.Item_7}  旧酒:{data.Item_8}");

            AudioManager_2.SoundPlay(3); // 播放打开音效

            itemManager.UpdateInventoryUI();//存档物品刷新




            // ===== 新增逻辑 =====
            // 1. 移除槽位上的皮肤（设置为 Item_x/Item_0）
            current[slotIndex] = 0;
            ApplySkins(); // 重新应用皮肤，slotIndex 对应的槽位会被清空

            // 2. 隐藏该槽位的购买按钮
            var button = all_ItemsIntroduce[id];
            if (button != null) button.SetActive(false);




            return true;
        }

        // 价格表
        private int GetPrice(int id)
        {
            switch (id)
            {
                case 1: return 100; // 跳蛋
                case 2: return 150; // 水晶
                case 3: return 200; // 均衡
                case 4: return 50; // 宝石
                case 5: return 100; // 酒瓶
                case 6: return 300; // 翡翠
                case 7: return 150; // 硬币
                case 8: return 250; // 旧酒
                default: return 0;
            }
        }
        #endregion

        /// <summary>
        /// 物品栏的选中介绍
        /// </summary>
        #region
        public List<GameObject> List_Item_Light; // 使用List来存储多个物品选中
        public List<GameObject> List_Item_Introduce; // 使用List来存储多个物品介绍

        public void Item_Setting(int Item_Number)
        {


            foreach (GameObject Light in List_Item_Light)
            {
                Light.SetActive(false);
            }
            foreach (GameObject Introduce in List_Item_Introduce)
            {
                Introduce.SetActive(false);
            }

            List_Item_Light[Item_Number].SetActive(true);
            List_Item_Introduce[Item_Number].SetActive(true);

            AudioManager.SoundPlay(0);
          

        }

        #endregion
    }
}