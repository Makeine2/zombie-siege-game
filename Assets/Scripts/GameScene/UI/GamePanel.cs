using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Image imgHP;
    public Text txtHP;
    public Text txtWave;
    public Text txtMoney;

    //hp的初始宽
    public float hpW = 500;
    public Button btnQuit;

    //
    public Transform botTrans;

    public List<TowerBtn> towerBtns = new List<TowerBtn>();

    //当前进入和选中的造塔点
    private TowerPoint nowSelTowerPoint;

    //是否检测造塔输入
    private bool checkInput;
    public override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            //隐藏游戏界面
            UIManager.Instance.HidePanel<GamePanel>();
            //返回到开始界面
            SceneManager.LoadScene("BeginScene");
            //其它


        });
        //一开始隐藏造塔相关的画面
        botTrans.gameObject.SetActive(false);
    }

    //提供给外部的血量更新函数
    public void UpdateTowerHp(int hp,int maxHp)
    {
        txtHP.text =  hp + "/" + maxHp;
        //更新血条的长度
        (imgHP.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHp * hpW, 38);
    }
    public void UpdateWaveNum(int nowNum,int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }
    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }
    public void UpdateSelTower(TowerPoint point)
    {
        //根据造塔点的信息 决定界面上的信息
        nowSelTowerPoint = point;

        if(nowSelTowerPoint == null)
        {
            checkInput = false;
            botTrans.gameObject.SetActive(false);
        }
        else
        {
            checkInput = true;
            botTrans.gameObject.SetActive(true);
            if (nowSelTowerPoint.nowTowerInfo == null)//没塔的时候
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPoint.chooseIDs[i], "数字键" + (i + 1));
                }

            }
            //如果造过塔
            else//塔升级
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);

                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.next, "空格键");
            }
        }
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Invoke() 会执行你之前在 Init() 里给 btnQuit 绑定的所有逻辑
            Cursor.lockState = CursorLockMode.None;
        }
        if (!checkInput)
        {
            return;
        }
        //主要用于造塔 键盘输入 造塔
        //如果没有造过塔 就检测1，2，3按键来造塔
        if(nowSelTowerPoint.nowTowerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
               
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[2]);
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("升级了");
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.next);
            }
        }
    }
}
