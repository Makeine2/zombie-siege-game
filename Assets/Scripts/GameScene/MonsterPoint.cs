using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //怪物多少波，多少只
    public int maxWave;

    public int monsterNumOneWave;
    //用于记录还有多少怪物没被创建
    private int nowNum;
    //怪物id
    public List<int> monsterIDs;
    private int nowID;
    //单只怪物间隔时间
    public float createOffsetTime;
    //波与波之间间隔时间
    public float delayTime;
    //第一波怪物创建的时间
    public float firstDelayTime;



    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave",firstDelayTime);

        //记录出怪点
        GameLeveMgr.Instance.AddMonsterPoint(this);
        //更新最大波数
        GameLeveMgr.Instance.UpdateMaxWave(maxWave);
    }

    private void CreateWave()
    {
        //得到当前波怪物的id是什么
        nowID = monsterIDs[Random.Range(0,monsterIDs.Count)];//左包含右不包含
        nowNum = monsterNumOneWave;
        //创建怪物
        CreateMonster(); 
        //减少波次
        --maxWave;
        //通知关卡管理器出了一波怪
        GameLeveMgr.Instance.ChangeNowWaveNum(1);
    }
    private void CreateMonster()
    {
        //直接创建怪物

        //取出怪物数据
        MonsterInfo info = GameDataMgr.Instance.monsterInfoList[nowID-1];

        //创建怪物预设体
        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res),this.transform.position,Quaternion.identity);
        MonsterObject monsterObj = obj.AddComponent<MonsterObject>();
        monsterObj.InitInfo(info);

        //告诉管理器怪物数量加1
        GameLeveMgr.Instance.AddMonster(monsterObj);


        --nowNum;
        if(nowNum == 0)
        {
            if(maxWave >0)
            {
                Invoke("CreateWave",delayTime);
            }
        }
        else
        {
            Invoke("CreateMonster", createOffsetTime);
        }
    }

    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
