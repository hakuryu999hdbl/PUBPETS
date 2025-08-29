using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Spine;
using UnityEngine;

public class ShopManagerSkin : MonoBehaviour
{
    /// <summary>
    /// 皮肤
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
        ShowCurrentAll(1,2,3,4);

    }

    public void ShowCurrentAll
       (
          int Item_1, int Item_2, int Item_3, int Item_4
       )
    {

        if (Item_1 != 0) { blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin($"Item_1/Item_0{Item_1}")); }
        if (Item_2 != 0) { blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin($"Item_2/Item_0{Item_2}")); }
        if (Item_3 != 0) { blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin($"Item_3/Item_0{Item_3}")); }
        if (Item_4 != 0) { blendSkin.AddSkin(skeletonAnimation.Skeleton.Data.FindSkin($"Item_4/Item_0{Item_4}")); }

        skeletonAnimation.Skeleton.SetSkin(blendSkin);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
    }
    #endregion


    /// <summary>
    /// 帧事件
    /// </summary>
    #region
    [Header("帧事件")]
    public GameObject Item_All;

    public void Open() 
    {
        Item_All.SetActive(true);
    }



    #endregion




}
