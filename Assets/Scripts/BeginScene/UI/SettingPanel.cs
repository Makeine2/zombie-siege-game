using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel//这里写这些逻辑就是监听这些UI控件
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;


    public override void Init()
    {
        //一开始要初始化
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn =  data.musicOpen;
        togSound.isOn = data.soundOpen;

        sliderMusic.value = data.musicValue;
        sliderSound.value = data.soundValue;

        btnClose.onClick.AddListener(() =>
        {
            //为了节省性能，只有当点了关闭键的时候才会用json保存数据到硬盘，写到文件中
            GameDataMgr.Instance.SaveMusicData();

            UIManager.Instance.HidePanel<SettingPanel>();
        });

        togMusic.onValueChanged.AddListener((v) =>
        {
            //这里这个v是看你这个控件是什么样的，比如说这是toggle多选框的框，那么它就是布尔值，看有没有选中这个框的
            //让背景音乐进行开关
            BKmusic.Instance.SetIsOpen(v);//相当于要修改什么都是通过给相应的单例主管说的
            //记录开关的数据
            GameDataMgr.Instance.musicData.musicOpen = v;

        });
        togSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.soundOpen = v;
        });
        sliderMusic.onValueChanged.AddListener((v) =>
        {
            BKmusic.Instance.ChangeValue(v);
            //记录数据
            GameDataMgr.Instance.musicData.musicValue = v;

        });
        sliderSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.soundValue = v;
        });


    }
}
