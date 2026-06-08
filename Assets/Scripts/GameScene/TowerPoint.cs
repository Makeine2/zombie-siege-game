using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    //造塔点关联的塔对象
    private GameObject towerObj = null;
    //造塔点关联的塔的数据
    public TowerInfo nowTowerInfo = null;

    //可以建造的三个塔的id
    public List<int> chooseIDs;
    private void Start()
    {

    }
    void Update()
    {
        //主要用于造塔
    }

    public void CreateTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id-1];
        //如果钱不够就不用建造了
        if(info.money > GameLeveMgr.Instance.player.money)
        {
            Debug.Log("钱不够");
            return;
        }
        GameLeveMgr.Instance.player.addMoney(-info.money);

        //创建塔
        //先判断是否有塔 有就删除
        if(towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        towerObj = Instantiate(Resources.Load<GameObject>(info.res),this.transform.position,Quaternion.identity);
        //初始化塔

        towerObj.GetComponent<TowerObject>().InitInfo(info);

        //
        nowTowerInfo = info;
        //塔建造完毕，更新游戏界面上的内容
        if(nowTowerInfo.next != 0)
        {
            Debug.Log("可以升级");
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else
        {
            Debug.Log("不可以升级");
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        //如果现在已经有塔了 就没必要再显示升级界面或者造塔界面了
        if (nowTowerInfo != null && nowTowerInfo.next == 0)//如果现在已经有塔了
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }
    private void OnTriggerExit(Collider other)
    {
        //如果不希望游戏界面下方的造塔界面显示 直接传空
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
    /// <summary>
    /// 更新当前选中造塔点界面的一些变化
    /// </summary>
    
    
}
