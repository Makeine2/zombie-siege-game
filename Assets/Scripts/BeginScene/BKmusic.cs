using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKmusic : MonoBehaviour
{
    private static BKmusic instance;
    public static BKmusic Instance => instance;

    private AudioSource bkSource;
    void Awake()//Awake()会在挂载脚本的物体一被激活就调用，在所有的start()之前
    {
        instance = this;//这里起到了类似构造函数的作用

        bkSource = this.GetComponent<AudioSource>();

        //通过数据来设置音乐大小和开关
        MusicData data = GameDataMgr.Instance.musicData;
        SetIsOpen(data.musicOpen);
        ChangeValue(data.musicValue);
    }

    //开关背景音乐的方法
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;//开启就说明不静音
    }

    public void ChangeValue(float v)
    {
        bkSource.volume = v;
    }
    //调整音乐大小的方法
}
