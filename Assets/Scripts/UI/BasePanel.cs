using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour//因为是只继承，不允许new的所以可以写成抽象类
{
    //专门用于控制面板透明度的组件
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float alphaSpeed = 2;
    public bool isShow = false;
    // Start is called before the first frame update

    //隐藏完毕后要做的事情
    private UnityAction HideCallBack = null;
    protected virtual void Awake()//为了确定能得到canvasGroup，写成虚函数就可以让子类重写
    {
        //获取面板对象的canvasgroup
        canvasGroup = this.GetComponent<CanvasGroup>();
        //如果忘记添加了
        if (canvasGroup == null )
        canvasGroup = this.AddComponent<CanvasGroup>();


    }
    protected virtual void Start()
    {
        Init();
    }
    public abstract void Init();//抽象方法无法声明主体，只能被继承
    
    public virtual void ShowMe()//显示自己
    {
        isShow = true;
        canvasGroup.alpha = 0;//显示自己的时候alpha为0即可,因为isShow这个会触发update里面的淡出效果了，这里只是初始为0，后面会逐渐加alphaSpeed的
    }

    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        HideCallBack = callBack;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        //处于显示状态时
        //淡入
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed*Time.deltaTime;//实现淡入淡出的效果
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }

        }
        else if(!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                HideCallBack?.Invoke();
            }
        }
    }
}
