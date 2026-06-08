using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;

    //这里是动画播放完毕之后要调用的事件
    private UnityAction overAction;
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    //给外部提供的方法 
    //左转
    public void TurnLeft(UnityAction action)
    {
        animator.SetTrigger("Left");//相当于这个触发器开枪了一次，就是触发这个名字的触发器一次
        overAction = action;
    }
    //右转
    public void TurnRight(UnityAction action)
    {
        animator.SetTrigger("Right");//相当于这个触发器开枪了一次，就是触发这个名字的触发器一次
        overAction = action;
    }
    //动画调用完之后的方法
    public void PlayerOver()
    {
        overAction?.Invoke();
        overAction = null;
    }



}
