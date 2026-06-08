using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnBack;
    public Button btnStart;

    public Text textInfo;
    public Image imgScene;

    //记录当前数据索引
    //记录当前的数据
    private int nowIndex;
    private SceneInfo nowSceneInfo;
    public override void Init()
    {
        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if(nowIndex < 0)
            {
                nowIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;
            }
            ChangeScene();
        });
        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if(nowIndex >= GameDataMgr.Instance.sceneInfoList.Count)
            {
                nowIndex = 0;
            }
            ChangeScene();
        });
        btnStart.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //切换场景
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);//这里因为要全部加载完才可以进行下一步，要不然它都找不到初始点

            ao.completed += (obj) =>
            {
                GameLeveMgr.Instance.InitInfo(nowSceneInfo);//在场景加载完的回调函来调用初始化数据
            };
           

        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //隐藏自己，显示选角色面板
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        //一打开面板也应该改变面板
        ChangeScene();


    }
    public void ChangeScene()
    {
        //切换界面的场景信息
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];

        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);

        textInfo.text = "名称\n" + nowSceneInfo.name + "\n" + "描述" +"\n" +nowSceneInfo.tips;
    }
}