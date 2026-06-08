using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    //管理器是单例对象
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    //存储显示着的面板 的字典
    //要隐藏面板时候就用它名字查找
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //场景中的canvas对象，用于设置为面板的父对象
    private Transform canvasTrans;

    private UIManager()
    {
        //得到canvas对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/canvas"));  
        canvasTrans = canvas.transform;
        //保证过场景的时候只有一个canvas
        GameObject.DontDestroyOnLoad(canvas);

    }
    //显示面板
    public T ShowPanel<T>() where T : BasePanel
    {
        //只需要保证泛型T 类型的名字和预设体的名字一样就可以了
        string panelName = typeof(T).Name;

        //判断字典中是否有
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        //根据面板名字 动态创建预设体 设置父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        //把这个对象放到场景中的canvas
        panelObj.transform.SetParent(canvasTrans, false);

        //指向面板上 显示逻辑 并且应该把它保存起来
        T panel = panelObj.GetComponent<T>();
        //加入到字典里面
        panelDic.Add(panelName, panel);
        //调用自己的显示逻辑
        panel.ShowMe();
        return panel;
    }

    //隐藏面板

    public void HidePanel<T>(bool isFade = true)where T : BasePanel
    {
        string panelName = typeof (T).Name;
        if(panelDic.ContainsKey(panelName))
        {
            if (isFade)//是否让面板淡出完毕后再删除
            {
                panelDic[panelName].HideMe(() =>
                {
                    //删除对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除字典里面的
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //删除对象
                GameObject.Destroy(panelDic[panelName].gameObject);
                //删除字典里面的
                panelDic.Remove(panelName);
            }
        }
        
    }
    //得到面版
    public T GetPanel<T>()where T : BasePanel
    {
        string name = typeof(T).Name;
        if (panelDic.ContainsKey(name))
        {
            return panelDic[(name)] as T;
        }
        else 
            return null;
    }

}
