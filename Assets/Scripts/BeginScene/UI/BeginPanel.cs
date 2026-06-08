using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnQuit;



    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //之后会在这里隐藏初始面板，然后显示选角色面板

            //先播放左转动画
            //因为我们的camera组件是依附在主摄像机上的，所以可以根据camera组件得到主摄像机的一切
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });
            //之后隐藏开始界面
            UIManager.Instance.HidePanel<BeginPanel>();

        });
        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();//发布后才有用
        });
    }
}
