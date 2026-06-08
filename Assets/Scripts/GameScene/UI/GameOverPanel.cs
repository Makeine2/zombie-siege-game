using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text txtWin;
    public Text txtInfo;
    public Text txtMoney;

    public Button btnSure;
    public override void Init()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        btnSure.onClick.AddListener(() =>
        {

            //隐藏面板
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.HidePanel<GameOverPanel>();
            //切换场景
            SceneManager.LoadScene("BeginScene");

        });
    }
    public void InitInfo(int money,bool isWin)
    {
        txtWin.text = isWin ? "通关" : "失败";
        txtInfo.text = isWin ? "胜利奖励" : "失败奖励";
        txtMoney.text ="$" +money;

        //根据奖励改变玩家数据
        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();



    }
}
