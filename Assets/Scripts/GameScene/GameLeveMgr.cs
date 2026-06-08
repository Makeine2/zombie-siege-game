using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLeveMgr
{ 
    private static GameLeveMgr instance = new GameLeveMgr();
    public static GameLeveMgr Instance => instance;

    public PlayerObject player;
    
    //所有的出怪点
    private List<MonsterPoint> points = new List<MonsterPoint>();

    //记录当前还有多少波
    private int nowWaveNum = 0;
    //记录一共有多少波怪物
    private int maxWaveNum;

    //private int nowMonsterNum = 0;

    //用于记录当前场景上的怪物的列表
    private List<MonsterObject> monsterList = new List<MonsterObject>();


    private GameLeveMgr()
    {

    }
    //切换游戏场景时候要动态创建玩家
    public void InitInfo(SceneInfo info)
    {
        //显示游戏界面
        UIManager.Instance.ShowPanel<GamePanel>();

        //玩家的创建
        //获取当前选中的玩家数据
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRol;
        //获取到场景中玩家的出生位置
        Transform heroPos = GameObject.Find("HeroBornPos").transform;


        

        //生成选中的人物
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),heroPos.position,heroPos.rotation);
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk, info.money);

        //让摄像机看向动态创建的玩家
        Camera.main.GetComponent<SmoothCameraFollow>().SetTarget(heroObj.transform);
 
    }
    //我们通过游戏管理器来 判断游戏是否胜利
    //要知道场景中是否有怪物没有出 以及场景中是否有还没有死掉的怪物
    //

    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }
    public void UpdateMaxWave(int num)
    {
        maxWaveNum = num;
        nowWaveNum = maxWaveNum;
        //更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum,maxWaveNum);

    }
    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        //更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }


    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        for(int i = 0; i < points.Count; i++)
        {
            //只要有一个出拐点没有出完怪，那就是还没有胜利
            if(!points[i].CheckOver())
            {
                return false;
            }
        }
        if (monsterList.Count > 0)
        {
            return false;
        }
        Debug.Log("游戏胜利");
        return true;
    }
    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }

    /// <summary>
    /// 将怪物从列表中移除
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }
   /* //改变当前场景怪物的数量
    public void ChangeMonsterNum(int num)
    {
        monsterList.Count += num;
    }*/
    /// <summary>
    /// 清空当前的数据，避免下一次的数据
    /// </summary>
    public void CleanInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
        player = null;
    }
    /// <summary>
    /// 寻找满足条件的怪物
    /// </summary>
    /// <param name="position"></param>
    /// <param name="Range"></param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 position,int Range)
    {
        for(int i = 0;i < monsterList.Count;i++)
        {
            //怪物列表中找到符合条件的怪物并且返回出去
            if (monsterList[i].isDead == false
                && Vector3.Distance(position, monsterList[i].transform.position) <= Range)
            {
                return monsterList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 寻找满足条件的所有怪物
    /// </summary>
    /// <param name="position"></param>
    /// <param name="Range"></param>
    /// <returns></returns>
    public List<MonsterObject> FindMonsters(Vector3 position, int Range)
    {
        //寻找所有满足条件的list
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            //怪物列表中找到符合条件的怪物并且返回出去
            if (monsterList[i].isDead == false
                && Vector3.Distance(position, monsterList[i].transform.position) <= Range)
            {
                list.Add(monsterList[i]);
            }
        }
        return list;


    }

}
