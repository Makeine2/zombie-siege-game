using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //左右键
    public Button btnLeft;
    public Button btnRight;

    //购买按钮
    public Button btnUnLock;
    public Text txtUnLock;

    //开始和返回
    public Button btnStart;
    public Button btnBack;

    //左上角有的钱
    public Text txtMoney;

    public Text txtName;
    private Transform HeroPos;

    private GameObject nowHero;
    private RoleInfo nowRoleData;
    //当前人物的索引
    private int nowIndex;
    public override void Init()//注册各个按键的事件
    {
        //找到场景对象预设体对象
        HeroPos = GameObject.Find("HeroPos").transform;

        //更新左上角的钱
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();

        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if(nowIndex < 0)
            {
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            }
            //模型的更新
            ChangeHero();
        });

        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.roleInfoList.Count)
            {
                nowIndex = 0;
            }
            //模型的更新
            ChangeHero();
        });

        btnUnLock.onClick.AddListener(() =>
        {
            PlayerData data = GameDataMgr.Instance.playerData;
            //如果钱够，就可以购买
            if(data.haveMoney > nowRoleData.lockMoney)
            {
                //购买逻辑
                //减去花费，更新界面显示
                data.haveMoney -= nowRoleData.lockMoney;
                txtMoney.text = data.haveMoney.ToString();
                data.buyHero.Add(nowRoleData.id);
                GameDataMgr.Instance.SavePlayerData();

                //更新解锁面板
                UpdateLockbtn();
                //提示面板显示购买成功
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功");
                
            }
            else//钱不够
            {
                //提示面板显示金钱不足
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金钱不足");
                
            }

        });
        btnStart.onClick.AddListener(() =>
        {
            //记录选择的英雄
            GameDataMgr.Instance.nowSelRol = nowRoleData;
            //隐藏自己，显示场景选择界面
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();

        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            //让摄像机转回去后 显示开始界面
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });

        ChangeHero();
    }//init()结尾

    private void ChangeHero()
    {
        if(nowHero != null)
        {
            Destroy(nowHero);
            nowHero = null;
        }
        //取出数据的一条
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];

        nowHero = Instantiate(Resources.Load<GameObject>(nowRoleData.res),HeroPos.position,HeroPos.rotation);
        txtName.text = nowRoleData.tips;
        //根据解锁相关数据，是否显示解锁按钮

        Destroy(nowHero.GetComponent<PlayerObject>());
        UpdateLockbtn();
    }//changehero

    private void UpdateLockbtn()
    {
        
        if (nowRoleData.lockMoney > 0 && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id)) 
        { //需要被解锁且玩家还没有解锁
            //更新解锁按钮显示
            btnUnLock.gameObject.SetActive(true);  
            txtUnLock.text = "$" +nowRoleData.lockMoney;//孩子们，这里是RoleInfo里的lockMoney写成了lockmoney导致数据读错了，以后注意啊
            //隐藏开始按钮 因为该角色没有解锁
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
        //Debug.Log(nowRoleData.lockmoney);
    }
    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        //隐藏自己时候就删掉角色
        if (nowHero != null)
        {
            DestroyImmediate(nowHero);
            nowHero = null;
        }
    }

}
