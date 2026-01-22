using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimController : MonoBehaviour
{
    //[SerializeField]
    private Animator animator;

    //private static readonly int OverHash = Animator.StringToHash("Over");

    public float ExitTime;

    private void Start()
    {
        if (!animator)
            animator = GetComponent<Animator>();

        Invoke(nameof(FinishDescription), ExitTime);
    }

    /// <summary>
    /// 描述结束时调用（文字播完 / 点击继续）
    /// </summary>
    public void FinishDescription()
    {
        //if (!animator) return;
        //animator.SetTrigger(OverHash);


        animator.SetTrigger("Over");
    }
}
